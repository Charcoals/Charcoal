using NUnit.Framework;

namespace PivotalTrackerDotNet.Tests {
	[TestFixture]
	public class StoryServiceTest {
		private StoryService storyService = null;

		[TestFixtureSetUp]
		public void TestFixtureSetUp() {
			storyService = new StoryService(AuthenticationService.Authenticate(TestCredentials.Username, TestCredentials.Password));
		}

		[Test]
		public void CanRetrieveSingleStory() {
			const int projectId = 456301;
			const int storyId = 23590427;
			var story = storyService.GetStory(projectId, storyId);
			Assert.NotNull(story);
			Assert.AreEqual(projectId, story.ProjectId);
			Assert.AreEqual(storyId, story.Id);
			Assert.AreEqual("feature", story.StoryType);
			Assert.AreEqual("Single Task", story.Name);
			Assert.AreEqual(2, story.Estimate);
		}

		[Test]
		public void CanRettrieveMultipleStories() {
			const int projectId = 456301;
			var stories = storyService.GetStories(projectId);
			Assert.NotNull(stories);
			Assert.AreEqual(2, stories.Count);
		}
	}
}