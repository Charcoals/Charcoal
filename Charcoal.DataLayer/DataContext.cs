using System;
using System.Linq;
using Charcoal.Core;
using Raven.Client;

namespace Charcoal.DataLayer {
    public class DataContext<T> : IDataContext<T> where T : BaseEntity {
        readonly IDocumentSession session;
        public DataContext(IDocumentSession session) {
            this.session = session;
        }

        public IQueryable<T> Query() {
            return session.Query<T>();
        }

        public T Load(string id) {
            return session.Load<T>(id);
        }

        public void Delete(T entity) {
            session.Delete(entity);
        }

        public void Store(T entity) {
            session.Store(entity, entity.ETag, entity.RavenKey());
        }

        public void SaveChanges() {
            session.SaveChanges();
        }
    }

    internal static class BaseEntityExtensions {
        internal static string RavenKey<T>(this T entity) where T : BaseEntity {
            var typeName = entity.GetType().Name.ToLower();
            var dataRoot = typeName.EndsWith("y") ? typeName.Replace("y", "ies") : typeName + "s";
            return string.Format("{0}/{1}", dataRoot, entity.Id);
        }
    }
}
