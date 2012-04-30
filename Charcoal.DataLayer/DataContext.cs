using System;
using System.Linq;
using Charcoal.Core;
using Raven.Client;

namespace Charcoal.DataLayer {
    public class DataContext : IDataContext {
        readonly IDocumentSession session;
        public DataContext(IDocumentSession session) {
            this.session = session;
        }

        public IQueryable<T> Query<T>() {
            return session.Query<T>();
        }

        public T Load<T>(string id) {
            return session.Load<T>(id);
        }

        public void Delete<T>(T entity) {
            session.Delete(entity);
        }

        public void Store<T>(T entity) where T : BaseEntity {
            session.Store(entity, entity.ETag, entity.Id);
        }

        public void SaveChanges() {
            session.SaveChanges();
        }
    }
}
