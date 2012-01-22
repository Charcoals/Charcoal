using System;
using PivotalTrackerDotNet.Domain;
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
		public void CanAddAndDeleteStores(){
			const int projectId = 456301;
			var story = new Story{Name="Nouvelle histoire", RequestedBy="pivotaltrackerdotnet",StoryType="feature", 
				Description="bla bla bla and more bla", ProjectId=projectId};
		    
			var savedStory = storyService.AddNewStory(projectId,story);
			Assert.AreEqual(story.Name, savedStory.Name);
			Assert.AreEqual(projectId, savedStory.ProjectId);
			Assert.AreEqual(story.RequestedBy, savedStory.RequestedBy);
			Assert.AreEqual(story.StoryType, savedStory.StoryType);
			Assert.AreEqual(story.Description, savedStory.Description);
			
			
			var deletedStory = storyService.RemoveStory(projectId, savedStory.Id);
			Assert.AreEqual(savedStory.Id, deletedStory.Id);
			Assert.AreEqual(savedStory.Name, deletedStory.Name);
			Assert.AreEqual(projectId, deletedStory.ProjectId);
			Assert.AreEqual(savedStory.RequestedBy, deletedStory.RequestedBy);
			Assert.AreEqual(savedStory.StoryType, deletedStory.StoryType);
			Assert.AreEqual(savedStory.Description, deletedStory.Description);
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
		
		[Test]
		public void CanAddAndDeleteNewTasks(){
			const int projectId = 456301;
			var story = new Story{Name="Nouvelle histoire", RequestedBy="pivotaltrackerdotnet",StoryType="feature", 
				Description="bla bla bla and more bla", ProjectId=projectId};
		    
			var savedStory = storyService.AddNewStory(projectId,story);
			
			var task = new Task{Description="stuff stuff stuff", ParentStoryId=savedStory.Id, ProjectId=projectId };
			
			var savedTask = storyService.AddNewTask(task);
			Assert.AreEqual(task.Description, savedTask.Description);
			
			var deletedTask = storyService.RemoveTask(task.ProjectId, task.ParentStoryId, savedTask.Id);
			Assert.NotNull(deletedTask);
			Assert.AreEqual(savedTask.Description, deletedTask.Description);
			Assert.AreEqual(savedTask.Id, deletedTask.Id);	
			
			storyService.RemoveStory(projectId, savedStory.Id);
		}
	}
}