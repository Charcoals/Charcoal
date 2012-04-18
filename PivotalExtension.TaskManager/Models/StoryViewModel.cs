using System.Linq;
using PivotalTrackerDotNet.Domain;

namespace Charcoal.Web.Models {
    public class StoryViewModel {

        public StoryViewModel(Story story) {
            Story = story;
        }

        public Story Story { get; set; }

        public string GetCardStyle() {
            return (Story.Notes.Any() ? "flippable " : "" ) + "story-card " + Story.CurrentState.ToString().ToLower();
        }

        public string GetHeader() {
            if (Story.StoryType == StoryType.Bug) {
                return "Bug";
            }

            if (Story.StoryType == StoryType.Chore) {
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
            if (Story.CurrentState == StoryStatus.UnStarted 
                || Story.CurrentState == StoryStatus.Finished 
                || Story.CurrentState == StoryStatus.Rejected) {
                return "start";
            }
            if (Story.CurrentState == StoryStatus.Started) {
                return "finish";
            }
            return string.Empty;
        }
    }
}