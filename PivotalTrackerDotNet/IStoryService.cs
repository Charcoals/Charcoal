using System.Collections.Generic;
using PivotalTrackerDotNet.Domain;

namespace PivotalTrackerDotNet {
	public interface IStoryService {
		Story AddNewStory(int projectId, Story toBeSaved);
		List<Story> GetCurrentStories(int projectId);
		List<Story> GetDoneStories(int projectId);
		List<Story> GetIceboxStories(int projectId);
		List<Story> GetBacklogStories(int projectId);
		List<Story> GetAllStories(int projectId);
		Story FinishStory(int projectId, int storyId);
		Story GetStory(int projectId, int storyId);
		Story RemoveStory(int projectId, int storyId);
		Task GetTask(int projectId, int storyId, int taskId);
		void SaveTask(Task task);
	}
}
