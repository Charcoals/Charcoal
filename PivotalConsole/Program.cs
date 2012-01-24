using System;
using PivotalTrackerDotNet;

namespace PivotalConsole {
	class Program {
		static void Main(string[] args) {
			var token = AuthenticationService.Authenticate("v5core", "changeme");
			var service = new StoryService(token);
			var stories = service.GetCurrentStories(424921);

			//var stories = Pivotal.Instance.GetCurrentStories();
			//foreach (var story in stories) {
			//  Console.WriteLine(story.Name);
			//  Console.WriteLine("---------------------------------");
			//  foreach (var task in story.Tasks) {
			//    Console.WriteLine(" - {0}", task.GetDescriptionWithoutOwners());
			//    foreach (var owner in task.GetOwners()) {
			//      Console.WriteLine("    owned by {0}", owner.Name);
			//    }
			//  }
			//  Console.WriteLine("---------------------------------");
			//}

			Console.ReadKey();
		}
	}
}