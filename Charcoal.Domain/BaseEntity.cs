using System;

namespace Charcoal.Core {
    public abstract class BaseEntity {
        public string Id { get; private set; }
        public Guid ETag { get; set; }
        public void SetId(Guid identifier) {
            var typeName = this.GetType().Name.ToLower();
            var dataRoot = typeName.EndsWith("y") ? typeName.Replace("y", "ies") : typeName + "s";
            Id = string.Format("{0}/{1}", dataRoot, identifier);
        }
    }
}
