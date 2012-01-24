using PivotalTrackerDotNet.Domain;

namespace PivotalExtension.TaskManager.Models {
	public class StoryRowViewModel {

		public StoryRowViewModel(Story story) {
			Story = story;
		}

		public Story Story { get; set; }
	}
}