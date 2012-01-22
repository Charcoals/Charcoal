using System.Web.Mvc;
using NUnit.Framework;
using PivotalExtension.TaskManager.Controllers;
using PivotalTrackerDotNet;
using PivotalTrackerDotNet.Domain;
using Rhino.Mocks;

namespace PivotalExtension.TaskManager.Tests {
    [TestFixture]
    public class TaskControllerTest {
        [Test]
        public void Details() {
            var mockery = new MockRepository();

            var projectId = 3;
            var storyId = 4;
            var id = 5;

            var storyService = mockery.StrictMock<IStoryService>();

            using (mockery.Record()) {
                Expect.Call(storyService.GetTask(projectId, storyId, id)).Return(new Task());
            }

            using (mockery.Playback()) {
                var controller = new TaskController(storyService);
                var result = controller.Details(id, storyId, projectId);
                var viewResult = result as PartialViewResult;
                Assert.NotNull(viewResult);
                Assert.AreEqual("TaskDetails", viewResult.ViewName);
                Assert.IsInstanceOf<Task>(viewResult.Model);
            }
        }
    }
}
