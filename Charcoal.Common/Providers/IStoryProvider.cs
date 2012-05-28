using System.Collections.Generic;
using Charcoal.Common.Entities;

namespace Charcoal.Common.Providers
{
	public interface IStoryProvider
	{
		Story AddNewStory(long projectId, Story toBeSaved);
        Task AddNewTask(Task task, long projectId);
		List<Story> GetStories(long projectId,IterationType iterationType);
		List<Story> GetAllStories(long projectId);
		Story FinishStory(long projectId, long storyId, IterationType iterationType);
		Story StartStory(long projectId, long storyId, IterationType iterationType);
		Story GetStory(long projectId, long storyId, IterationType iterationType);
		Story RemoveStory(long projectId, long storyId);
		Task GetTask(long projectId, long storyId, long taskId);
		bool RemoveTask(long projectId, long storyId, long taskId);
        void SaveTask(Task task, long projectId);
		OperationResponse UpdateTask(Task task, long projectId);
		void ReorderTasks(long projectId, long storyId, List<Task> tasks);
		void AddComment(long projectId, long storyId, string comment);
	}
}