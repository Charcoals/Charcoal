using System;
using System.Collections.Generic;
using Simple.Data;

namespace Charcoal.DataLayer
{
    public class ProjectRepository : IRepository
    {

        private readonly string m_connectionString;

        internal ProjectRepository(string connectionString)
        {
            m_connectionString = connectionString;
        }

        public DatabaseOperationResponse Save(dynamic entity)
        {
            try
            {
                var database = Database.OpenConnection(m_connectionString);
                database.Projects.Insert(entity);
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
                if (database.Projects.FindById(entity.Id) == null)
                {
                    return new DatabaseOperationResponse(false, "Item Does not exist", FailReason.ItemNoLongerExists);
                }

                var inserted = database.Projects.Update(entity);
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
                if (database.Projects.FindById(id) == null)
                {
                    return new DatabaseOperationResponse(false, "Item Does not exist", FailReason.ItemNoLongerExists);
                }

                var deleted = database.Projects.DeleteById(id);
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
            return database.Projects.All().ToList();
        }

        public dynamic Find(long id)
        {
            var database = Database.OpenConnection(m_connectionString);
            return database.Projects.FindById(id);
        }
    }
}