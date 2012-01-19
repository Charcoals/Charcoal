using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace PivotalTrackerDotNet.Domain {
	public class Task {
		public int Id { get; set; }
		public string Description { get; set; }
		public bool Complete { get; set; }
		public int ParentStoryId { get; set; }
		public int ProjectId { get; set; }

		static readonly AuthenticationToken Token = AuthenticationService.Authenticate("v5core", "changeme");
		protected static List<Person> Members;

		Regex FullOwnerRegex = new Regex(@"([ ]?\-[ ]?)?(\()?[A-Z]{2,3}(\/[A-Z]{2,3})*(\))?");

		public string GetDescriptionWithoutOwners() {
			var descriptionWithoutOwners = FullOwnerRegex.Replace(Description, "");
			return descriptionWithoutOwners.Length == 0 ? "(Placeholder)" : descriptionWithoutOwners;
		}

		public void SetOwners(List<Person> owners) {
			if (owners.Count == 0) return;

			var match = FullOwnerRegex.Match(Description);
			if (match != null) {
				Description = Description.Remove(match.Index);
				var initials = string.Join("/", owners.Select(o => o.Initials));
				Description += " - " + initials;
			}
		}

		public List<Person> GetOwners() {
			if (Members == null) {
				Members = new MembershipService(Token).GetMembers(ProjectId);
			}

			var owners = new List<Person>();
			if (Complete) return owners;

			var regex = new Regex(@"[A-Z]{2,3}(\/[A-Z]{2,3})+");
			var matches = regex.Matches(Description);

			if (matches.Count > 0) {
				var membersLookup = Members.ToDictionary(m => m.Initials);
				var initials = matches[0].Value.Split('/');
				foreach (var owner in initials) {
					if (membersLookup.ContainsKey(owner)) {
						owners.Add(membersLookup[owner]);
					}
				}
			}

			return owners;
		}

		public string GetStyle() {
			if (this.Complete) {
				return "task complete";
			}
			else if (this.GetOwners().Any()) {
				return "task in-progress";
			}
			else {
				return "task";
			}
		}
	}

	//  <?xml version="1.0" encoding="UTF-8"?>
	//<task>
	//  <id type="integer">$TASK_ID</id>
	//  <description>find shields</description>
	//  <position>1</position>
	//  <complete>false</complete>
	//  <created_at type="datetime">2008/12/10 00:00:00 UTC</created_at>
	//</task>
}