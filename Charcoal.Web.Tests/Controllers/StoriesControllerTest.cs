using System.Collections.Generic;
using Charcoal.Common.Entities;
using Charcoal.Web.Controllers;
using Charcoal.Web.Models;
using NUnit.Framework;
using System.Web.Mvc;
using Moq;
using Charcoal.Common.Providers;

namespace Charcoal.Web.Tests.Controllers
{
    [TestFixture]
    public class StoriesControllerTest
    {
        [Test]
        public void CurrentIteration()
        {

            int projectId = 3;
            var storyService = new Mock<IStoryProvider>();


            storyService.Setup(e => e.GetStories(It.Is<long>(je => je == 3), IterationType.Current)).Returns(new List<Story>());

            var controller = new StoriesController(storyService.Object);
            var result = controller.CurrentIteration(projectId);
            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult);
            Assert.IsInstanceOf<ProjectStoryViewModel>(viewResult.Model);
            storyService.Verify();
        }

        [Test]
        public void BacklogStories()
        {
            int projectId = 3;
            var storyService = new Mock<IStoryProvider>();


            storyService.Setup(e => e.GetStories(It.Is<long>(je => je == 3), IterationType.Backlog)).Returns(new List<Story>());

            var controller = new StoriesController(storyService.Object);
            var result = controller.BackLogStories(projectId);
            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult);
            Assert.IsInstanceOf<ProjectStoryViewModel>(viewResult.Model);
            storyService.Verify();
        }

        [Test]
        public void IceboxStories()
        {
            int projectId = 3;
            var storyService = new Mock<IStoryProvider>();


            storyService.Setup(e => e.GetStories(It.Is<long>(je => je == projectId), IterationType.Icebox)).Returns(new List<Story>());

            var controller = new StoriesController(storyService.Object);
            var result = controller.IceboxStories(projectId);
            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult);
            Assert.IsInstanceOf<ProjectStoryViewModel>(viewResult.Model);
            storyService.Verify();
        }

        [Test]
        public void Get()
        {
            int projectId = 3;
            int storyId = 5;
            var storyService = new Mock<IStoryProvider>();

            var story = new Story();
            IterationType iterationType = IterationType.Backlog;
            storyService.Setup(e => e.GetStory(It.Is<long>(je => je == projectId), It.Is<long>(je => je == storyId), iterationType)).Returns(story);

            var controller = new StoriesController(storyService.Object);
            var result = controller.Get(projectId, storyId, (int)iterationType);
            var viewResult = result as PartialViewResult;
            Assert.NotNull(viewResult);
            Assert.IsInstanceOf<StoryRowViewModel>(viewResult.Model);
            Assert.AreEqual(((StoryRowViewModel)viewResult.Model).Story, story);
            storyService.Verify();
        }

        [Test]
        public void Start()
        {
            var storyService = new Mock<IStoryProvider>();

            var storyId = 1;
            var projectId = 3;

            var anotherStory = new Story() { Status = StoryStatus.Started };
            var iterationType = IterationType.Current;
            storyService.Setup(e => e.StartStory(projectId, storyId, iterationType)).Returns(anotherStory);

            var controller = new StoriesController(storyService.Object);
            var result = controller.Start(projectId, storyId, (int)iterationType);
            var viewResult = result as PartialViewResult;
            Assert.NotNull(viewResult);
            Assert.IsInstanceOf<StoryRowViewModel>(viewResult.Model);
            Assert.AreEqual(((StoryRowViewModel)viewResult.Model).Story, anotherStory);
            storyService.Verify();
        }

        [Test]
        public void Finish()
        {

            var storyService = new Mock<IStoryProvider>();

            var storyId = 1;
            var projectId = 3;

            var anotherStory = new Story() { Status = StoryStatus.Finished };

            IterationType iterationType = IterationType.Undefined;
            storyService.Setup(e => e.FinishStory(projectId, storyId, iterationType)).Returns(anotherStory);
            var controller = new StoriesController(storyService.Object);
            var result = controller.Finish(projectId, storyId, (int)iterationType);
            var viewResult = result as PartialViewResult;
            Assert.NotNull(viewResult);
            Assert.IsInstanceOf<StoryRowViewModel>(viewResult.Model);
            Assert.AreEqual(((StoryRowViewModel)viewResult.Model).Story, anotherStory);
            storyService.Verify();
        }

        //[Test]
        //public void AddComment()
        //{

        //    var storyService = new Mock<IStoryProvider>();

        //    var storyId = 1;
        //    var projectId = 3;
        //    var comment = "comment";

        //    var story = new Story();

        //        storyService.AddComment(projectId, storyId, comment);
        //        Expect.Call(storyService.GetStory(projectId, storyId)).Return(story);
        //        var controller = new StoriesController(storyService);
        //        var result = controller.AddComment(projectId, storyId, comment);

        //        var partialResult = result as PartialViewResult;
        //        Assert.NotNull(partialResult);
        //        var model = partialResult.Model as StoryRowViewModel;
        //        Assert.NotNull(model);
        //        Assert.AreEqual(story, model.Story);
        //    storyService.Verify();
        //}
    }
}
