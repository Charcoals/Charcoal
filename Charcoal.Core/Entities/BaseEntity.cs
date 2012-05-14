using System;

namespace Charcoal.Core.Entities
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }
    }

    [Flags]
    public enum Privilege
    {
        Undefined = 0,
        Admin,
        Product,
        Developer,
        Tester,
        All
    }

    public enum StoryStatus { UnScheduled, UnStarted, Started, Finished, Delivered, Accepted, Rejected }
}
