using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace PivotalConnect {
	public class Task {
		public int Id { get; set; }
		public string Description { get; set; }
		public bool Complete { get; set; }

		Regex FullOwnerRegex = new Regex(@"([ ]?\-[ ]?)?(\()?[A-Z]{2,3}(\/[A-Z]{2,3})+(\))?");

		public string GetDescriptionWithoutOwners() {
			return FullOwnerRegex.Replace(Description, "");
		}

		public void SetOwners(List<Member> owners) {
			if (owners.Count == 0) return;

			var match = FullOwnerRegex.Match(Description);
			if (match != null) {
				Description = Description.Remove(match.Index);
				var initials = string.Join("/", owners.Select(o => o.Initials));
				Description += " - " + initials;
			}
		}

		public List<Member> GetOwners() {
			var owners = new List<Member>();
			if (Complete) return owners;

			var regex = new Regex(@"[A-Z]{2,3}(\/[A-Z]{2,3})+");
			var matches = regex.Matches(Description);

			if (matches.Count > 0) {
				var membersLookup = Pivotal.Members.ToDictionary(m => m.Initials);
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
}