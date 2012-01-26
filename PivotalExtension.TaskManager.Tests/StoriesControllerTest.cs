using System.Collections.Generic;
using NUnit.Framework;
using PivotalExtension.TaskManager.Models;
using Rhino.Mocks;
using PivotalTrackerDotNet;
using PivotalTrackerDotNet.Domain;
using PivotalExtension.TaskManager.Controllers;
using System.Web.Mvc;

namespace PivotalExtension.TaskManager.Tests {
    [TestFixture]
    public class StoriesControllerTest {
        [Test]
        public void CurrentIteration () {
            var mockery = new MockRepository ();

            var projectId = 3;
            var storyService = mockery.StrictMock<IStoryService> ();

            using (mockery.Record ()) {
                Expect.Call (storyService.GetCurrentStories (projectId)).Return (new List<Story> ());
            }

            using (mockery.Playback ()) {
                var controller = new StoriesController (storyService);
                var result = controller.CurrentIteration (projectId);
                var viewResult = result as ViewResult;
                Assert.NotNull (viewResult);
                Assert.IsInstanceOf<List<Story>> (viewResult.Model);
            }
        }

    	[Test]
    	public void BacklogStories() {
					var mockery = new MockRepository();

					var projectId = 3;
					var storyService = mockery.StrictMock<IStoryService>();

					using (mockery.Record()) {
						Expect.Call(storyService.GetBacklogStories(projectId)).Return(new List<Story>());
					}

					using (mockery.Playback()) {
						var controller = new StoriesController(storyService);
						var result = controller.BackLogStories(projectId);
						var viewResult = result as ViewResult;
						Assert.NotNull(viewResult);
						Assert.IsInstanceOf<List<Story>>(viewResult.Model);
					}
    	}

        [Test]
        public void Get () {
            var mockery = new MockRepository ();
            var storyService = mockery.StrictMock<IStoryService> ();

            var storyId = 1;
            var projectId = 3;

            var story = new Story ();

            using (mockery.Record ()) {
                Expect.Call (storyService.GetStory (projectId, storyId)).Return (story);
            }

            using (mockery.Playback ()) {
                var controller = new StoriesController (storyService);
                var result = controller.Get (projectId, storyId);
                var viewResult = result as PartialViewResult;
                Assert.NotNull (viewResult);
                Assert.IsInstanceOf<StoryRowViewModel> (viewResult.Model);
                Assert.AreEqual (((StoryRowViewModel)viewResult.Model).Story, story);
            }
        }
    }
}
