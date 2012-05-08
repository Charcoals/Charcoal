using System;
using System.Collections.Generic;
using Charcoal.DataLayer.Entities;
using Simple.Data;

namespace Charcoal.DataLayer
{
    public class ProjectRepository : IRepository<Project>
    {

        private readonly string m_connectionString;

        internal ProjectRepository(string connectionString)
        {
            m_connectionString = connectionString;
        }

        public DatabaseOperationResponse Save(Project entity)
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

        public DatabaseOperationResponse DeepSave(Project entity)
        {
            throw new NotImplementedException();
        }

        public DatabaseOperationResponse Update(Project entity)
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

        public List<Project> FindAll()
        {
            var database = Database.OpenConnection(m_connectionString);
            return database.Projects.All().ToList<Project>();
        }

        public Project Find(long id)
        {
            var database = Database.OpenConnection(m_connectionString);
            return database.Projects.FindById(id);
        }
    }
}