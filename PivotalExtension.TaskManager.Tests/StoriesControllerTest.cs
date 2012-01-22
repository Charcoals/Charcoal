using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using PivotalTrackerDotNet;
using PivotalTrackerDotNet.Domain;
using PivotalExtension.TaskManager.Controllers;
using System.Web.Mvc;

namespace PivotalExtension.TaskManager.Tests {
    [TestFixture]
    public class StoriesControllerTest {
        [Test]
        public void Index() {
            var mockery = new MockRepository();

            var projectId = 3;
            var storyService = mockery.StrictMock<IStoryService>();

            using (mockery.Record()) {
                Expect.Call(storyService.GetStories(projectId)).Return(new List<Story>());
            }

            using (mockery.Playback()) {
                var controller = new StoriesController(storyService);
                var result = controller.CurrentIteration(projectId);
                var viewResult = result as ViewResult;
                Assert.NotNull(viewResult);
                Assert.IsInstanceOf<List<Story>>(viewResult.Model);
            }
        }
    }
}
