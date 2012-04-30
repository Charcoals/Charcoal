using System.Linq;

namespace Charcoal.DataLayer {
    internal interface IDataContext<T> {
        IQueryable<T> Query();
        T Load(string id);
        void Delete(T entity);
        void Store(T entity);
        void SaveChanges();
    }
}