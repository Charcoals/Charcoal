using System.Collections.Generic;
using Charcoal.Common;
using Charcoal.Common.Entities;
using Charcoal.Common.Providers;
using Charcoal.DataLayer;

namespace Charcoal.Core
{
    public class CharcoalStoryProvider:IStoryProvider
    {
        private IStoryRepository m_storyRepository;

        public CharcoalStoryProvider(IStoryRepository storyRepository = null)
        {
            m_storyRepository = storyRepository ?? new StoryRepository();
        }

        public Story AddNewStory(long projectId, Story toBeSaved)
        {
            toBeSaved.ProjectId = projectId;
            var response = m_storyRepository.Save(toBeSaved);
            return response.HasSucceeded ? response.Object : null;
        }

        public Task AddNewTask(Task task, long projectId)
        {
            throw new System.NotImplementedException();
        }

        public List<Story> GetStories(long projectId, IterationType iterationType)
        {
            return m_storyRepository.FindAllByIterationType(projectId, (int) iterationType);
        }

        public List<Story> GetAllStories(long projectId)
        {
            return m_storyRepository.FindAllByProjectId(projectId);
        }

        public Story FinishStory(long projectId, long storyId, IterationType iterationType)
        {
            throw new System.NotImplementedException();
        }

        public Story StartStory(long projectId, long storyId, IterationType iterationType)
        {
            throw new System.NotImplementedException();
        }

        public Story GetStory(long projectId, long storyId, IterationType iterationType)
        {
            throw new System.NotImplementedException();
        }

        public Story RemoveStory(long projectId, long storyId)
        {
            throw new System.NotImplementedException();
        }

        public Task GetTask(long projectId, long storyId, long taskId)
        {
            throw new System.NotImplementedException();
        }

        public bool RemoveTask(long projectId, long storyId, long taskId)
        {
            throw new System.NotImplementedException();
        }

        public void SaveTask(Task task, long projectId)
        {
            throw new System.NotImplementedException();
        }

        public OperationResponse UpdateTask(Task task, long projectId)
        {
            throw new System.NotImplementedException();
        }

        public void ReorderTasks(long projectId, long storyId, List<Task> tasks)
        {
            throw new System.NotImplementedException();
        }

        public void AddComment(long projectId, long storyId, string comment)
        {
            throw new System.NotImplementedException();
        }
    }
}