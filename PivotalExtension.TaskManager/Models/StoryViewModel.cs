using System.Web.Mvc;
using PivotalTrackerDotNet.Domain;

namespace PivotalExtension.TaskManager.Models {
    public class StoryViewModel {

        public StoryViewModel(Story story) {
            Story = story;
        }

        public Story Story { get; set; }

        public string GetStyle() {
            return "story-details " + Story.CurrentState;
        }

        public string GetHeader() {
            if (Story.StoryType == "bug") {
                return "Bug";
            }

            if (Story.StoryType == "chore") {
                return "Spike / Misc";
            }

            return "Story";
        }

        public string GetEstimate() {
            if (Story.Estimate > 0) {
                return Story.Estimate + " points";
            }

            return string.Empty;
        }

        public string FormattedId {
            get { return Story.ProjectId + "-" + Story.Id; }
        }

        public string AdvanceAction() {
            if (Story.CurrentState == StoryStatus.UnStarted || Story.CurrentState == StoryStatus.Finished) {
                return "start";
            }
            if (Story.CurrentState == StoryStatus.Started) {
                return "finish";
            }
            return string.Empty;
        }
    }
}