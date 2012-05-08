using System;
using System.Collections.Generic;
using Simple.Data;
using Charcoal.DataLayer.Entities;

namespace Charcoal.DataLayer
{
    public class UserRepository : IRepository<User>
    {

        private readonly string m_connectionString;

        internal UserRepository(string connectionString)
        {
            m_connectionString = connectionString;
        }

        public DatabaseOperationResponse Save(User entity)
        {
            try
            {
                var database = Database.OpenConnection(m_connectionString);
                database.Users.Insert(entity);
                return new DatabaseOperationResponse(true);
            }
            catch (Exception ex)
            {
                return new DatabaseOperationResponse(description: ex.Message, reason: FailReason.Exception);
            }
        }

        public DatabaseOperationResponse DeepSave(User entity)
        {
            throw new NotImplementedException();
        }

        public DatabaseOperationResponse Update(User entity)
        {
            try
            {
                var database = Database.OpenConnection(m_connectionString);
                if (database.Users.FindById(entity.Id) == null)
                {
                    return new DatabaseOperationResponse(false, "Item Does not exist", FailReason.ItemNoLongerExists);
                }

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

        public List<User> FindAll()
        {
            var database = Database.OpenConnection(m_connectionString);
            return database.Users.All()
                .With(database.Users.UsersXProjects.Projects.As("Projects"))
                .ToList<User>();
        }

        public User Find(long id)
        {
            var database = Database.OpenConnection(m_connectionString);
            return database.Users.FindAllById(id)
                .With(database.Users.UsersXProjects.Projects.As("Projects"))
                .FirstOrDefault();
        }
    }
}