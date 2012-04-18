using System.Collections.Generic;
using Charcoal.Web.Controllers;
using Charcoal.Web.Models;
using NUnit.Framework;
using Rhino.Mocks;
using PivotalTrackerDotNet;
using PivotalTrackerDotNet.Domain;
using System.Web.Mvc;

namespace PivotalExtension.TaskManager.Tests.Controllers {
    [TestFixture]
    public class StoriesControllerTest {
        [Test]
        public void CurrentIteration() {
            var mockery = new MockRepository();

            var projectId = 3;
            var storyService = mockery.StrictMock<IStoryService>();

            using (mockery.Record()) {
                Expect.Call(storyService.GetCurrentStories(projectId)).Return(new List<Story>());
            }

            using (mockery.Playback()) {
                var controller = new StoriesController(storyService);
                var result = controller.CurrentIteration(projectId);
                var viewResult = result as ViewResult;
                Assert.NotNull(viewResult);
                Assert.IsInstanceOf<List<Story>>(viewResult.Model);
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
        public void IceboxStories() {
            var mockery = new MockRepository();

            var projectId = 3;
            var storyService = mockery.StrictMock<IStoryService>();

            using (mockery.Record()) {
                Expect.Call(storyService.GetIceboxStories(projectId)).Return(new List<Story>());
            }

            using (mockery.Playback()) {
                var controller = new StoriesController(storyService);
                var result = controller.IceboxStories(projectId);
                var viewResult = result as ViewResult;
                Assert.NotNull(viewResult);
                Assert.IsInstanceOf<List<Story>>(viewResult.Model);
            }
        }

        [Test]
        public void Get() {
            var mockery = new MockRepository();
            var storyService = mockery.StrictMock<IStoryService>();

            var storyId = 1;
            var projectId = 3;

            var story = new Story();

            using (mockery.Record()) {
                Expect.Call(storyService.GetStory(projectId, storyId)).Return(story);
            }

            using (mockery.Playback()) {
                var controller = new StoriesController(storyService);
                var result = controller.Get(projectId, storyId);
                var viewResult = result as PartialViewResult;
                Assert.NotNull(viewResult);
                Assert.IsInstanceOf<StoryRowViewModel>(viewResult.Model);
                Assert.AreEqual(((StoryRowViewModel)viewResult.Model).Story, story);
            }
        }

        [Test]
        public void Start() {
            var mockery = new MockRepository();
            var storyService = mockery.StrictMock<IStoryService>();

            var storyId = 1;
            var projectId = 3;

            var anotherStory = new Story() { CurrentState = StoryStatus.Started };
            using (mockery.Record()) {
                Expect.Call(storyService.StartStory(projectId, storyId)).Return(anotherStory);
            }

            using (mockery.Playback()) {
                var controller = new StoriesController(storyService);
                var result = controller.Start(projectId, storyId);
                var viewResult = result as PartialViewResult;
                Assert.NotNull(viewResult);
                Assert.IsInstanceOf<StoryRowViewModel>(viewResult.Model);
                Assert.AreEqual(((StoryRowViewModel)viewResult.Model).Story, anotherStory);
            }
        }

        [Test]
        public void Finish() {
            var mockery = new MockRepository();
            var storyService = mockery.StrictMock<IStoryService>();

            var storyId = 1;
            var projectId = 3;

            var anotherStory = new Story() { CurrentState = StoryStatus.Finished };
            using (mockery.Record()) {
                Expect.Call(storyService.FinishStory(projectId, storyId)).Return(anotherStory);
            }

            using (mockery.Playback()) {
                var controller = new StoriesController(storyService);
                var result = controller.Finish(projectId, storyId);
                var viewResult = result as PartialViewResult;
                Assert.NotNull(viewResult);
                Assert.IsInstanceOf<StoryRowViewModel>(viewResult.Model);
                Assert.AreEqual(((StoryRowViewModel)viewResult.Model).Story, anotherStory);
            }
        }

        [Test]
        public void AddComment() {
            var mockery = new MockRepository();
            var storyService = mockery.StrictMock<IStoryService>();

            var storyId = 1;
            var projectId = 3;
            var comment = "comment";

            var story = new Story();

            using (mockery.Record()) 
            using (mockery.Ordered()) {
                storyService.AddComment(projectId, storyId, comment);
                Expect.Call(storyService.GetStory(projectId, storyId)).Return(story);
            }

            using(mockery.Playback()){
                var controller = new StoriesController(storyService);
                var result = controller.AddComment(projectId, storyId, comment);

                var partialResult = result as PartialViewResult;
                Assert.NotNull(partialResult);
                var model = partialResult.Model as StoryRowViewModel;
                Assert.NotNull(model);
                Assert.AreEqual(story, model.Story);
            }
        }
    }
}
