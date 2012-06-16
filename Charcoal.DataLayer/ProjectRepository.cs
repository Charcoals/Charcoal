using System;
using System.Collections.Generic;
using System.Configuration;
using Simple.Data;

namespace Charcoal.DataLayer
{
    public interface IProjectRepository : IRepository
    {
        List<dynamic> GetProjectsByUseToken(string apiToken);
        DatabaseOperationResponse CreateProjectAssociatedWithKey(dynamic project, string apiToken);
        DatabaseOperationResponse AddUserToProject(long projectId, string userName);
    }

    public class ProjectRepository : IProjectRepository
    {

        private readonly string m_connectionString;

        internal ProjectRepository(string connectionString)
        {
            m_connectionString = connectionString;
        }

        public ProjectRepository()
            : this(ConfigurationManager.ConnectionStrings["Server"].ConnectionString)
        {

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

        public List<dynamic> FindAll()
        {
            var database = Database.OpenConnection(m_connectionString);
            return database.Projects.All()
                .With(database.Projects.UsersXProjects.Users.As("Users"))
                .ToList();
        }

        public dynamic Find(long id)
        {
            var database = Database.OpenConnection(m_connectionString);
            return database.Projects.FindById(id);
        }

        public List<dynamic> GetProjectsByUseToken(string apiToken)
        {
            var database = Database.OpenConnection(m_connectionString);

            var userId = database.Users.FindByAPIKey(apiToken).Id;

            return database.Projects.FindAll(database.Projects.UsersXProjects.Users.Id == userId).ToList<dynamic>();
        }

        public DatabaseOperationResponse CreateProjectAssociatedWithKey(dynamic project, string apiToken)
        {
            try
            {
                var database = Database.OpenConnection(m_connectionString);
                var userId = database.Users.FindByAPIKey(apiToken).Id;
                var savedProject = database.Projects.Insert(project);

                database.UsersXProjects.Insert(UserId: userId, ProjectId: savedProject.Id);
                return new DatabaseOperationResponse(true);

            }
            catch (Exception ex)
            {
                return new DatabaseOperationResponse(description: ex.Message);
            }

        }

        public DatabaseOperationResponse AddUserToProject(long projectId, string userName)
        {
            throw new NotImplementedException();
        }
    }
}