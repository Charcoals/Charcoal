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

        [Test]
        public void SignUp() {
            var mockery = new MockRepository();

            var projectId = 3;
            var storyId = 4;
            var id = 5;
            var initials = "NN/GZ";
            var description = "Doin work";
            var task = new Task { Description = description, Id = id, ParentStoryId = storyId, ProjectId = projectId };

            var storyService = mockery.StrictMock<IStoryService>();

            using (mockery.Record())
            using (mockery.Ordered()) {
                Expect.Call(storyService.GetTask(projectId, storyId, id)).Return(task);
                storyService.SaveTask(task);
            }

            using (mockery.Playback()) {
                var controller = new TaskController(storyService);
                var result = controller.SignUp(id, storyId, projectId, initials);
                var viewResult = result as PartialViewResult;
                Assert.NotNull(viewResult);
                Assert.AreEqual("TaskDetails", viewResult.ViewName);
                Assert.IsInstanceOf<Task>(viewResult.Model);
                var modelTask = viewResult.Model as Task;
                Assert.AreEqual(projectId, modelTask.ProjectId);
                Assert.AreEqual(storyId, modelTask.ParentStoryId);
                Assert.AreEqual(id, modelTask.Id);
                Assert.AreEqual(string.Format("{0} ({1})", description, initials), modelTask.Description);
            }
        }

        [Test]
        public void SignUp_Already_Has_Initials() {
            var mockery = new MockRepository();

            var projectId = 3;
            var storyId = 4;
            var id = 5;
            var initials = "NN/GZ";
            var description = "Doin work (AA/FF)";
            var task = new Task { Description = description, Id = id, ParentStoryId = storyId, ProjectId = projectId };

            var storyService = mockery.StrictMock<IStoryService>();

            using (mockery.Record())
            using (mockery.Ordered()) {
                Expect.Call(storyService.GetTask(projectId, storyId, id)).Return(task);
                storyService.SaveTask(task);
            }

            using (mockery.Playback()) {
                var controller = new TaskController(storyService);
                var result = controller.SignUp(id, storyId, projectId, initials);
                var viewResult = result as PartialViewResult;
                Assert.NotNull(viewResult);
                Assert.AreEqual("TaskDetails", viewResult.ViewName);
                Assert.IsInstanceOf<Task>(viewResult.Model);
                var modelTask = viewResult.Model as Task;
                Assert.AreEqual(projectId, modelTask.ProjectId);
                Assert.AreEqual(storyId, modelTask.ParentStoryId);
                Assert.AreEqual(id, modelTask.Id);
                Assert.AreEqual(string.Format("{0} ({1})", description.Replace(" (AA/FF)", ""), initials), modelTask.Description);
            }
        }

        [Test]
        public void SignUp_Already_Has_Initials_NoParentheses() {
            var mockery = new MockRepository();

            var projectId = 3;
            var storyId = 4;
            var id = 5;
            var initials = "NN/GZ";
            var description = "Doin work AA/FF";
            var task = new Task { Description = description, Id = id, ParentStoryId = storyId, ProjectId = projectId };

            var storyService = mockery.StrictMock<IStoryService>();

            using (mockery.Record())
            using (mockery.Ordered()) {
                Expect.Call(storyService.GetTask(projectId, storyId, id)).Return(task);
                storyService.SaveTask(task);
            }

            using (mockery.Playback()) {
                var controller = new TaskController(storyService);
                var result = controller.SignUp(id, storyId, projectId, initials);
                var viewResult = result as PartialViewResult;
                Assert.NotNull(viewResult);
                Assert.AreEqual("TaskDetails", viewResult.ViewName);
                Assert.IsInstanceOf<Task>(viewResult.Model);
                var modelTask = viewResult.Model as Task;
                Assert.AreEqual(projectId, modelTask.ProjectId);
                Assert.AreEqual(storyId, modelTask.ParentStoryId);
                Assert.AreEqual(id, modelTask.Id);
                Assert.AreEqual(string.Format("{0} ({1})", description.Replace(" AA/FF", ""), initials), modelTask.Description);
            }
        }

        [Test]
        public void SignUp_No_Initials_Clears_Existing() {
            var mockery = new MockRepository();

            var projectId = 3;
            var storyId = 4;
            var id = 5;
            var initials = "";
            var description = "Doin work (AA/FF)";
            var task = new Task { Description = description, Id = id, ParentStoryId = storyId, ProjectId = projectId };

            var storyService = mockery.StrictMock<IStoryService>();

            using (mockery.Record())
            using (mockery.Ordered()) {
                Expect.Call(storyService.GetTask(projectId, storyId, id)).Return(task);
                storyService.SaveTask(task);
            }

            using (mockery.Playback()) {
                var controller = new TaskController(storyService);
                var result = controller.SignUp(id, storyId, projectId, initials);
                var viewResult = result as PartialViewResult;
                Assert.NotNull(viewResult);
                Assert.AreEqual("TaskDetails", viewResult.ViewName);
                Assert.IsInstanceOf<Task>(viewResult.Model);
                var modelTask = viewResult.Model as Task;
                Assert.AreEqual(projectId, modelTask.ProjectId);
                Assert.AreEqual(storyId, modelTask.ParentStoryId);
                Assert.AreEqual(id, modelTask.Id);
                Assert.AreEqual(string.Format("{0}", description.Replace(" (AA/FF)", "")), modelTask.Description);
            }
        }
    }
}
