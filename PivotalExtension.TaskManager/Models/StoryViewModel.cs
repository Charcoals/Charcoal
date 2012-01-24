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
	}
}