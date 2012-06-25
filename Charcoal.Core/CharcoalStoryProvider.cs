using System.Collections.Generic;
using Charcoal.Common;
using Charcoal.Common.Entities;
using Charcoal.Common.Providers;
using Charcoal.DataLayer;

namespace Charcoal.Core
{
    public class CharcoalStoryProvider : IStoryProvider
    {
        private readonly ITaskRepository m_taskRepository;
        private readonly IStoryRepository m_storyRepository;

        public CharcoalStoryProvider(IStoryRepository storyRepository = null, ITaskRepository taskRepository = null)
        {
            m_taskRepository = taskRepository ?? new TaskRepository();
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
            var response = m_taskRepository.Save(task);
            return response.HasSucceeded ? response.Object : null;
        }

        public List<Story> GetStories(long projectId, IterationType iterationType)
        {
            return m_storyRepository.FindAllByIterationType(projectId, (int)iterationType).ConvertAll(e=> (Story)e);
        }

        public List<Story> GetAllStories(long projectId)
        {
            return m_storyRepository.FindAllByProjectId(projectId).ConvertAll(e => (Story)e);
        }

        public List<Story> GetAllStoriesByTag(long projectId, string tag)
        {
            throw new System.NotImplementedException();
        }

        public Story FinishStory(long projectId, long storyId, IterationType iterationType)
        {
            return m_storyRepository.UpdateStoryStatus(storyId, (int)StoryStatus.Finished);
        }

        public Story StartStory(long projectId, long storyId, IterationType iterationType)
        {
            return m_storyRepository.UpdateStoryStatus(storyId, (int)StoryStatus.Started);
        }

        public Story GetStory(long projectId, long storyId, IterationType iterationType)
        {
            return m_storyRepository.Find(storyId);
        }

        public bool RemoveStory(long projectId, long storyId)
        {
            return m_storyRepository.Delete(storyId).HasSucceeded;
        }

        public Task GetTask(long projectId, long storyId, long taskId)
        {
            return m_taskRepository.Find(taskId);
        }

        public bool RemoveTask(long projectId, long storyId, long taskId)
        {
            return m_taskRepository.Delete(taskId).HasSucceeded;
        }

        public OperationResponse UpdateTask(Task task, long projectId)
        {
            var res = m_taskRepository.Update(task);
            return new OperationResponse(res.HasSucceeded, res.Description);
        }

        public void ReorderTasks(long projectId, long storyId, List<Task> tasks)
        {
            m_taskRepository.Save(tasks);
        }

        public void AddComment(long projectId, long storyId, string comment)
        {
            throw new System.NotImplementedException();
        }

        public List<Iteration> GetRecentIterations(long projectId, int number)
        {
            throw new System.NotImplementedException();
        }
    }
}