using PivotalTrackerDotNet.Domain;

namespace PivotalTrackerDotNet {
    public interface IStoryService {
        Story AddNewStory(int projectId, Story toBeSaved);
        System.Collections.Generic.List<Story> GetStories(int projectId);
        Story GetStory(int projectId, int storyId);
        Story RemoveStory(int projectId, int storyId);
        Task GetTask(int projectId, int storyId, int taskId);
        void SaveTask(Task task);
    }
}
