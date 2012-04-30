using System.Linq;
using Charcoal.Core;

namespace Charcoal.DataLayer {
    internal interface IDataContext {
        IQueryable<T> Query<T>();
        T Load<T>(string id);
        void Delete<T>(T entity);
        void Store<T>(T entity) where T : BaseEntity;
        void SaveChanges();
    }
}