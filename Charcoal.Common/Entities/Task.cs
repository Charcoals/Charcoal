using System;

namespace Charcoal.Common.Entities
{
    public class Task : BaseEntity
    {
        public string Description { get; set; }
        public string Assignees { get; set; }
        public long StoryId { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastEditedOn { get; set; }
        public Story Story { get; set; }
        public int Position { get; set; }
    }
}