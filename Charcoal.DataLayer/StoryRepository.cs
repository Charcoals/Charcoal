using System;
using System.Collections.Generic;
using System.Configuration;
using Simple.Data;

namespace Charcoal.DataLayer
{
    public interface IStoryRepository:IRepository
    {
        List<dynamic> FindAllByIterationType(long projectId, int iterationType);
        List<dynamic> FindAllByProjectId(long projectId);
        dynamic UpdateStoryStatus(long storyId, int status);
    }

    public class StoryRepository : IStoryRepository
    {
        private readonly string m_connectionString;

        public StoryRepository()
            : this(ConfigurationManager.ConnectionStrings["Server"].ConnectionString)
        {
            
        }

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
                var result = database.Stories.Insert(entity);
                return new DatabaseOperationResponse(true){Object = result};
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

        public List<dynamic> FindAll()
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
            return GetStory(id, database);
        }

        public List<dynamic> FindAllByIterationType(long projectId, int iterationType)
        {
            var database = Database.OpenConnection(m_connectionString);
            return database.Stories.FindAll(database.Stories.IterationType == iterationType 
                                        && database.Stories.ProjectId == projectId)
               .With(database.Stories.Projects.As("Project"))
               .WithTasks(database.Stories.Id == database.Tasks.StoryId)
                .ToList();
        
        }

        public List<dynamic> FindAllByProjectId(long projectId)
        {
            var database = Database.OpenConnection(m_connectionString);
            return database.Stories.FindAll(database.Stories.ProjectId == projectId)
               .With(database.Stories.Projects.As("Project"))
               .WithTasks(database.Stories.Id == database.Tasks.StoryId)
                .ToList();
        }

        public dynamic UpdateStoryStatus(long storyId, int status)
        {
            var database = Database.OpenConnection(m_connectionString);
            int updates= database.Stories.UpdateById(Id: storyId, Status: status);
            return updates == 1 ? GetStory(storyId, database) : null;
        }

        private static dynamic GetStory(long id, dynamic database)
        {
            return database.Stories.FindAllById(id)
                .With(database.Stories.Projects.As("Project"))
                .WithTasks(database.Stories.Id == database.Tasks.StoryId)
                .FirstOrDefault();
        }
    }
}