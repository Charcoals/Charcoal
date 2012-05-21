using System;
using System.Collections.Generic;

namespace Charcoal.Common.Entities
{
    public class Story : BaseEntity
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public StoryStatus Status { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastEditedOn { get; set; }
        public long ProjectId { get; set; }
        public Project Project { get; set; }
        public List<Task> Tasks { get; set; }

        public int? Estimate { get; set; }
        public StoryType StoryType { get; set; }
        public IterationType IterationType { get; set; }
    }
    public enum StoryType {Feature, Bug, Chore }
    public enum IterationType{Undefined=0,Current, Backlog, Icebox}
}