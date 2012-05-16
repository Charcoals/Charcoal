using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Charcoal.Common.Entities;

namespace Charcoal.Web.Models {
    public class TaskViewModel {
        private readonly long m_projectId;

        public TaskViewModel(Task task, long projectId, IterationType iterationType)
        {
            m_projectId = projectId;
            Task = task;
            IterationType = iterationType;
        }

        public Task Task { get; set; }
        public IterationType IterationType { get; private set; }

        static Regex FullOwnerRegex = new Regex(@"([ ]?\-[ ]?)?(\()?[A-Z]{2,3}(\/[A-Z]{2,3})*(\))?$", RegexOptions.Compiled);

        public string GetDescriptionWithoutOwners() {
            var placeholder = "(Placeholder)";
            if (Task.Description == null) return placeholder;
            var descriptionWithoutOwners = FullOwnerRegex.Replace(Task.Description, "");
            return descriptionWithoutOwners.Length == 0 ? placeholder : descriptionWithoutOwners.TrimEnd();
        }

        public List<string> GetOwners()
        {
            return new List<string>();
            //return Task.Assignees.Split('/').ToList();
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

        public long ProjectId { get { return m_projectId; } }

        public long StoryId { get { return Task.StoryId; } }

        public long Id { get { return Task.Id; } }

        public bool Complete { get { return Task.IsCompleted; } }

        public string Description { get { return Task.Description; } }

        public object DisplayId { get { return string.Format("{0}-{1}-{2}-{3}", ProjectId, StoryId, Id, (int)IterationType); } }

        public string DisplayStoryId { get { return string.Format("{0}-{1}", ProjectId, StoryId); } }
    }
}