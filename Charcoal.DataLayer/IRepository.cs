using System.Collections.Generic;

namespace Charcoal.DataLayer
{
    public interface IRepository
    {
        DatabaseOperationResponse Save(dynamic entity);
        DatabaseOperationResponse DeepSave(dynamic entity);
        DatabaseOperationResponse Update(dynamic entity);
        DatabaseOperationResponse Delete(long id);

        List<dynamic> FindAll();
        dynamic Find(long id);
    }
}
