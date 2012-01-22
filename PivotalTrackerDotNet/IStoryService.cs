using System;
namespace PivotalTrackerDotNet {
    public interface IStoryService {
        PivotalTrackerDotNet.Domain.Story AddNewStory(int projectId, PivotalTrackerDotNet.Domain.Story toBeSaved);
        System.Collections.Generic.List<PivotalTrackerDotNet.Domain.Story> GetStories(int projectId);
        PivotalTrackerDotNet.Domain.Story GetStory(int projectId, int storyId);
        PivotalTrackerDotNet.Domain.Story RemoveStory(int projectId, int storyId);
        void SaveTask(PivotalTrackerDotNet.Domain.Task task);
    }
}
