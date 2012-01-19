using System.Collections.Generic;

namespace PivotalConnect
{
	public class Story {
		public int Id { get; set; }
		public StoryType Type { get; set; }
		public string Name { get; set; }
		public List<Task> Tasks { get; set; }
	}
}