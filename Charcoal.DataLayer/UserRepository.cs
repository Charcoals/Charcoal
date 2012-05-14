using System;
using System.Collections.Generic;
using System.Configuration;
using Simple.Data;

namespace Charcoal.DataLayer
{
    public interface IUserRepository:IRepository
    {
        dynamic FindByEmail(string email);
        dynamic FindByUserName(string name);
        bool IsValid(string userName, string password);
    }

    public class UserRepository : IUserRepository
    {

        private readonly string m_connectionString;

        public UserRepository()
            : this(ConfigurationManager.ConnectionStrings["Server"].ConnectionString)
        {
            
        }

        internal UserRepository(string connectionString)
        {
            m_connectionString = connectionString;
        }

        public DatabaseOperationResponse Save(dynamic entity)
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

        public DatabaseOperationResponse DeepSave(dynamic entity)
        {
            throw new NotImplementedException();
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

        public dynamic FindAll()
        {
            var database = Database.OpenConnection(m_connectionString);
            return database.Users.All()
                .With(database.Users.UsersXProjects.Projects.As("Projects"))
                .ToList();
        }

        public dynamic Find(long id)
        {
            var database = Database.OpenConnection(m_connectionString);
            return database.Users.FindAllById(id)
                .With(database.Users.UsersXProjects.Projects.As("Projects"))
                .FirstOrDefault();
        }

        public dynamic FindByEmail(string email)
        {
            var database = Database.OpenConnection(m_connectionString);
            return database.Users.FindByEmail(email);
        }

        public dynamic FindByUserName(string name)
        {
            var database = Database.OpenConnection(m_connectionString);
            return database.Users.FindByUserName(name);
        }

        public bool IsValid(string userName, string password)
        {
            var database = Database.OpenConnection(m_connectionString);
            return database.Users.Find(database.Users.UserName == userName
                                       && database.Users.Password == password) != null;
        }
    }
}