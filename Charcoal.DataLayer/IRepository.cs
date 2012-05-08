using System.Collections.Generic;
using Charcoal.DataLayer.Entities;

namespace Charcoal.DataLayer
{
    public interface IRepository<T> where T : BaseEntity
    {
        DatabaseOperationResponse Save(T entity);
        DatabaseOperationResponse DeepSave(T entity);

        DatabaseOperationResponse Delete(long id);

        List<T> FindAll();
        T Find(long id);
    }
}
