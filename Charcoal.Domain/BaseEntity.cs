using System;

namespace Charcoal.Core {
    public abstract class BaseEntity {
        public int Id { get; set; }
        public Guid ETag { get; set; }
    }
}
