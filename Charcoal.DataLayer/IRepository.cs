using System.Collections.Generic;

namespace Charcoal.DataLayer
{
    public interface IRepository
    {
        DatabaseOperationResponse Save(dynamic entity);
        DatabaseOperationResponse Save(IEnumerable<dynamic> entities);

        DatabaseOperationResponse Delete(long id);

        List<dynamic> FindAll();
        dynamic Find(long id);
    }
}
