using System;
using System.Collections.Generic;
using Simple.Data;

namespace Charcoal.DataLayer
{
    public class StoryRepository:IRepository
    {
        private readonly string m_connectionString;

        internal StoryRepository(string connectionString)
        {
            m_connectionString = connectionString;
        }

        public OperationResponse Save(dynamic entity)
        {
            try
            {
                entity.CreatedOn = DateTime.UtcNow;
                entity.LastEditedOn = DateTime.UtcNow;
                var database = Database.OpenConnection(m_connectionString);
                database.Stories.Insert(entity);
                return new OperationResponse(true);
            }
            catch (Exception ex)
            {

                return new OperationResponse(description: ex.Message);
            }
        }

        public OperationResponse Save(IEnumerable<dynamic> entities)
        {
            //   using(var transaction = db.BeginTransaction() )
            throw new System.NotImplementedException();
        }

        public OperationResponse Update(dynamic entity)
        {
            throw new System.NotImplementedException();
        }

        public OperationResponse Delete(dynamic entity)
        {
            throw new System.NotImplementedException();
        }

        public OperationResponse Delete(IEnumerable<dynamic> entity)
        {
            throw new System.NotImplementedException();
        }

        public List<dynamic> FindAll()
        {
            throw new System.NotImplementedException();
        }

        public dynamic Find(long id)
        {
            throw new System.NotImplementedException();
        }
    }
}