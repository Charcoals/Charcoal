using System.Collections.Generic;

namespace Charcoal.DataLayer
{
    public interface IRepository
    {
        DatabaseOperationResponse Save(dynamic entity);
        DatabaseOperationResponse DeepSave(dynamic entity);

        DatabaseOperationResponse Delete(long id);

        dynamic FindAll();
        dynamic Find(long id);
    }
}
