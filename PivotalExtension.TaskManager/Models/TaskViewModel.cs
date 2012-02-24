using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using PivotalTrackerDotNet.Domain;
using PivotalTrackerDotNet;

namespace PivotalExtension.TaskManager.Models {
    public class TaskViewModel {

        public TaskViewModel(Task task) {
            Task = task;
        }

        public Task Task { get; set; }

        static Regex FullOwnerRegex = new Regex(@"([ ]?\-[ ]?)?(\()?[A-Z]{2,3}(\/[A-Z]{2,3})*(\))?$", RegexOptions.Compiled);

        public string GetDescriptionWithoutOwners() {
            var placeholder = "(Placeholder)";
            if (Task.Description == null) return placeholder;
            var descriptionWithoutOwners = FullOwnerRegex.Replace(Task.Description, "");
            return descriptionWithoutOwners.Length == 0 ? placeholder : descriptionWithoutOwners.TrimEnd();
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

        static Regex regex = new Regex(@"\([A-Z]{2,3}(\/[A-Z]{2,3})*\)", RegexOptions.Singleline | RegexOptions.Compiled);

        public List<string> GetOwners() {
            var matches = regex.Matches(Task.Description);
            if (matches.Count == 0) return new List<string>();
            return matches[0].Value
                .TrimStart('(').TrimEnd(')')
                .Split('/').ToList();
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

        public int ProjectId { get { return Task.ProjectId; } }

        public int StoryId { get { return Task.ParentStoryId; } }

        public int Id { get { return Task.Id; } }

        public bool Complete { get { return Task.Complete; } }

        public string Description { get { return Task.Description; } }

        public object DisplayId { get { return string.Format("{0}-{1}-{2}", ProjectId, StoryId, Id); } }

        public string DisplayStoryId { get { return string.Format("{0}-{1}", ProjectId, StoryId); } }
    }
}