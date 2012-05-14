using System.Collections.Generic;
using Charcoal.Common.Entities;

namespace Charcoal.Common.Providers
{
	public interface IStoryProvider
	{
		Story AddNewStory(long projectId, Story toBeSaved);
		Task AddNewTask(Task task);
		List<Story> GetStories(long projectId,StoryStatus storyStatus);
		List<Story> GetAllStories(long projectId);
		Story FinishStory(long projectId, long storyId);
		Story StartStory(long projectId, long storyId);
		Story GetStory(long projectId, long storyId);
		Story RemoveStory(long projectId, long storyId);
		Task GetTask(long projectId, long storyId, long taskId);
		Task RemoveTask(long projectId, long storyId, long taskId);
		OperationResponse UpdateTask(Task task);
		void ReorderTasks(long projectId, long storyId, List<Task> tasks);
		void AddComment(long projectId, long storyId, string comment);
	}
}