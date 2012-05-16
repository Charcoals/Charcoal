
using Charcoal.Common.Entities;

namespace Charcoal.Web.Models {
	public class StoryRowViewModel {

		public StoryRowViewModel(Story story,IterationType iterationType) {
			Story = story;
		    IterationType = iterationType;
		}

		public Story Story { get; private set; }
        public IterationType IterationType { get; private set; }
	}
}