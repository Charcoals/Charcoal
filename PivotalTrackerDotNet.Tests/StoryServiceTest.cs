using System;
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
			Assert.AreEqual(1, story.Tasks.Count);
			Assert.AreEqual(storyId, story.Tasks[0].ParentStoryId);

		}

		[Test]
		public void CanRettrieveMultipleStories() {
			const int projectId = 456301;
			var stories = storyService.GetStories(projectId);
			Assert.NotNull(stories);
			Assert.AreEqual(2, stories.Count);
		}

		[Test]
		public void CanSaveTask() {
			const int projectId = 456301;
			const int storyId = 23590427;

			var guid = Guid.NewGuid().ToString();

			var stories = storyService.GetStories(456301);
			var task = stories[0].Tasks[0];
			task.Description = guid;

			storyService.SaveTask(task);

			stories = storyService.GetStories(456301);

			Assert.AreEqual(guid, stories[0].Tasks[0].Description);
		}
	}
}