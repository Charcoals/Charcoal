using System;
using System.Collections.Generic;
using Simple.Data;

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

        public DatabaseOperationResponse DeepSave(dynamic entity)
        {
            throw new NotImplementedException();
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

        public dynamic FindAll()
        {
            var database = Database.OpenConnection(m_connectionString);
            return database.Stories.All()
                .With(database.Stories.Projects.As("Project"))
                .WithTasks(database.Stories.Id == database.Tasks.StoryId)
                .ToList();
        }

        public dynamic Find(long id)
        {
            var database = Database.OpenConnection(m_connectionString);
            return database.Stories.FindAllById(id)
                .With(database.Stories.Projects.As("Project"))
                .WithTasks(database.Stories.Id == database.Tasks.StoryId)
                .FirstOrDefault();
        }
    }
}