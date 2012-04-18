using PivotalTrackerDotNet.Domain;

namespace Charcoal.Web.Models {
	public class StoryRowViewModel {

		public StoryRowViewModel(Story story) {
			Story = story;
		}

		public Story Story { get; set; }
	}
}