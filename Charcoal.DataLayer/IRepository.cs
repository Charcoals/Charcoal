using System.Collections.Generic;

namespace Charcoal.DataLayer
{
    public interface IRepository
    {
        OperationResponse Save(dynamic entity);
        OperationResponse Save(IEnumerable<dynamic> entities);

        OperationResponse Update(dynamic entity);

        OperationResponse Delete(dynamic entity);
        OperationResponse Delete(IEnumerable<dynamic> entity);

        List<dynamic> FindAll();
        dynamic Find(long id);
    }
}
