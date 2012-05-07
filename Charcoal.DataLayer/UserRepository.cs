using System;
using System.Collections.Generic;
using Simple.Data;

namespace Charcoal.DataLayer
{
    public class UserRepository:IRepository{

        private readonly string m_connectionString;

        internal UserRepository(string connectionString)
        {
            m_connectionString = connectionString;
        }

        public DatabaseOperationResponse Save(dynamic entity)
        {
            try
            {
                entity.CreatedOn = DateTime.UtcNow;
                entity.LastEditedOn = DateTime.UtcNow;
                var database = Database.OpenConnection(m_connectionString);
                database.Users.Insert(entity);
                return new DatabaseOperationResponse(true);
            }
            catch (Exception ex)
            {
                return new DatabaseOperationResponse(description: ex.Message, reason: FailReason.Exception);
            }
        }

        public DatabaseOperationResponse Save(IEnumerable<dynamic> entities)
        {
            try
            {
                var database = Database.OpenConnection(m_connectionString);
                using (var tx = database.BeginTransaction())
                {
                    foreach (var entity in entities)
                    {
                        tx.Users.Insert(entity);
                    }
                    tx.Commit();
                    return new DatabaseOperationResponse(true);
                }
            }
            catch (Exception ex)
            {

                return new DatabaseOperationResponse(description: ex.Message, reason: FailReason.Exception);
            }
        }

        public DatabaseOperationResponse Update(dynamic entity)
        {
            try
            {
                var database = Database.OpenConnection(m_connectionString);
                if (database.Users.FindById(entity.Id) == null)
                {
                    return new DatabaseOperationResponse(false, "Item Does not exist", FailReason.ItemNoLongerExists);
                }
                entity.LastEditedOn = DateTime.UtcNow;

                var inserted = database.Users.Update(entity);
                return new DatabaseOperationResponse(inserted == 1);
            }
            catch (Exception ex)
            {
                return new DatabaseOperationResponse(description: ex.Message, reason: FailReason.Exception);
            }
        }

        public DatabaseOperationResponse Delete(long id)
        {
            try
            {
                var database = Database.OpenConnection(m_connectionString);
                if (database.Users.FindById(id) == null)
                {
                    return new DatabaseOperationResponse(false, "Item Does not exist", FailReason.ItemNoLongerExists);
                }

                var deleted = database.Users.DeleteById(id);
                return new DatabaseOperationResponse(deleted == 1);
            }
            catch (Exception ex)
            {
                return new DatabaseOperationResponse(description: ex.Message, reason: FailReason.Exception);
            }
        }

        public List<dynamic> FindAll()
        {
            var database = Database.OpenConnection(m_connectionString);
            return database.Users.All().ToList<dynamic>();
        }

        public dynamic Find(long id)
        {
            var database = Database.OpenConnection(m_connectionString);
            return database.Users.FindById(id);
        }
    }
}