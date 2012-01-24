using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using PivotalTrackerDotNet.Domain;
using PivotalTrackerDotNet;

namespace PivotalExtension.TaskManager.Models {
    public class TaskViewModel {
        public static IMembershipService service;//ugh
        IMembershipService Service {
            get {
                if (service == null){
                    service = new MembershipService((AuthenticationToken)HttpContext.Current.Session["token"]);
                }
                return service;
            }
        }

        public TaskViewModel(Task task) {
            Task = task;
        }

        public TaskViewModel(Task task, IMembershipService service) : this(task) {
            TaskViewModel.service = service;
        }

        public Task Task { get; set; }

        protected static List<Person> Members;

        static Regex FullOwnerRegex = new Regex(@"([ ]?\-[ ]?)?(\()?[A-Z]{2,3}(\/[A-Z]{2,3})*(\))?$", RegexOptions.Compiled);

        public string GetDescriptionWithoutOwners() {
            var descriptionWithoutOwners = FullOwnerRegex.Replace(Task.Description, "");
            return descriptionWithoutOwners.Length == 0 ? "(Placeholder)" : descriptionWithoutOwners.TrimEnd();
        }

        public void SetOwners(List<Person> owners) {
            if (owners.Count == 0) return;

            var match = FullOwnerRegex.Match(Task.Description);
            if (match != null) {
                Task.Description = Task.Description.Remove(match.Index);
                var initials = string.Join("/", owners.Select(o => o.Initials));
                Task.Description += " - " + initials;
            }
        }

        void PopulateMembers() {
            Members = Service.GetMembers(Task.ProjectId);
        }

        public List<Person> GetOwners() {
            if (Members == null) {
                PopulateMembers();
            }

            var owners = new List<Person>();

            var regex = new Regex(@"[A-Z]{2,3}(\/[A-Z]{2,3})+");
            var matches = regex.Matches(Task.Description);

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

        //TODO: Remove passthrough of token
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

        public int ProjectId { get { return Task.ProjectId; } }

        public int ParentStoryId { get { return Task.ParentStoryId; } }

        public int Id { get { return Task.Id; } }

        public bool Complete { get { return Task.Complete; } }

        public string Description { get { return Task.Description; } }

        public object DisplayId { get { return string.Format("{0}-{1}-{2}", ProjectId, ParentStoryId, Id); } }
    }
}