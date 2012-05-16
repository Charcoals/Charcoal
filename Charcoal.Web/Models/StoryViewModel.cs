using System.Linq;
using Charcoal.Common.Entities;

namespace Charcoal.Web.Models {
    public class StoryViewModel {

        public StoryViewModel(Story story, IterationType iterationType)
        {
            Story = story;
            IterationType = iterationType;
        }

        public Story Story { get; private set; }
        public IterationType IterationType { get; private set; }

        public string GetCardStyle() {
            return "story-card " + Story.Status.ToString().ToLower();
            //return (Story.Notes.Any() ? "flippable " : "" ) + "story-card " + Story.CurrentState.ToString().ToLower();
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
            get { return Story.ProjectId + "-" + Story.Id + "-" + (int)IterationType; }
        }

        public string AdvanceAction() {
            if (Story.Status == StoryStatus.UnStarted
                || Story.Status == StoryStatus.Finished
                || Story.Status == StoryStatus.Rejected)
            {
                return "start";
            }
            if (Story.Status == StoryStatus.Started)
            {
                return "finish";
            }
            return string.Empty;
        }
    }
}