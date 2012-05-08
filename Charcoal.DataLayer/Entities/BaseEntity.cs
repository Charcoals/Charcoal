using System;

namespace Charcoal.DataLayer.Entities
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }
    }

    [Flags]
    public enum Privilege
    {
        Undefined = 0,
        Read,
        Write,
        All
    }

    public enum StoryStatus { UnScheduled, UnStarted, Started, Finished, Delivered, Accepted, Rejected }
}
