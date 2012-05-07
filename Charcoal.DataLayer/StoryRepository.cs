using System;
using System.Collections.Generic;
using Simple.Data;
using System.Linq;

namespace Charcoal.DataLayer
{
    public class StoryRepository : IRepository
    {
        private readonly string m_connectionString;

        internal StoryRepository(string connectionString)
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
                database.Stories.Insert(entity);
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
                        tx.Stories.Insert(entity);
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
                if (database.Stories.FindById(entity.Id) == null)
                {
                    return new DatabaseOperationResponse(false, "Item Does not exist", FailReason.ItemNoLongerExists);
                }
                entity.LastEditedOn = DateTime.UtcNow;

                var inserted = database.Stories.Update(entity);
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
                if (database.Stories.FindById(id) == null)
                {
                    return new DatabaseOperationResponse(false, "Item Does not exist", FailReason.ItemNoLongerExists);
                }

                var deleted = database.Stories.DeleteById(id);
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
            return database.Stories.All().WithTasks(database.Stories.Id == database.Tasks.StoryId).ToList<dynamic>();
        }

        public dynamic Find(long id)
        {
            var database = Database.OpenConnection(m_connectionString);
            return database.Stories.FindAllById(id).WithTasks(database.Stories.Id == database.Tasks.StoryId).FirstOrDefault();
        }
    }
}