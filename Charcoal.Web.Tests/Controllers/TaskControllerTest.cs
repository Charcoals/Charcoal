﻿using System;
using System.Web.Mvc;
using Charcoal.Web.Controllers;
using Charcoal.Web.Models;
using NUnit.Framework;
using PivotalTrackerDotNet;
using PivotalTrackerDotNet.Domain;
using Rhino.Mocks;
using System.Collections.Generic;

namespace Charcoal.Web.Tests.Controllers {
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
                Assert.IsInstanceOf<TaskViewModel>(viewResult.Model);
            }
        }

        //[Test]
        //public void SignUp () {
        //    var mockery = new MockRepository ();

        //    var projectId = 3;
        //    var storyId = 4;
        //    var id = 5;
        //    var initials = "NN/GZ";
        //    var description = "Doin work";
        //    var task = new Task { Description = description, Id = id, ParentStoryId = storyId, ProjectId = projectId };

        //    var storyService = mockery.StrictMock<IStoryProvider> ();

        //    using (mockery.Record ())
        //    using (mockery.Ordered ()) {
        //        Expect.Call (storyService.GetTask (projectId, storyId, id)).Return (task);
        //        storyService.SaveTask (task);
        //    }

        //    using (mockery.Playback ()) {
        //        var controller = new TaskController (storyService);
        //        var result = controller.SignUp (id, storyId, projectId, initials);
        //        var viewResult = result as PartialViewResult;
        //        Assert.NotNull (viewResult);
        //        Assert.AreEqual ("TaskDetails", viewResult.ViewName);
        //        Assert.IsInstanceOf<TaskViewModel> (viewResult.Model);
        //        var modelTask = viewResult.Model as TaskViewModel;
        //        Assert.AreEqual (projectId, modelTask.ProjectId);
        //        Assert.AreEqual (storyId, modelTask.StoryId);
        //        Assert.AreEqual (id, modelTask.Id);
        //        Assert.AreEqual (string.Format ("{0} ({1})", description, initials), modelTask.Description);
        //    }
        //}

        [Test]
        public void SignUp_Multiple() {
            var mockery = new MockRepository();

            var projectId = 3;
            var storyId = 4;
            var id = 5;
            var id2 = 6;
            var initials = "NN/GZ";
            var description = "Doin work";
            var task = new Task { Description = description, Id = id, ParentStoryId = storyId, ProjectId = projectId };
            var task2 = new Task { Description = description, Id = id2, ParentStoryId = storyId, ProjectId = projectId };

            var storyService = mockery.StrictMock<IStoryService>();

            using (mockery.Record())
            using (mockery.Ordered()) {
                Expect.Call(storyService.GetTask(projectId, storyId, id)).Return(task);
                storyService.SaveTask(task);
                Expect.Call(storyService.GetTask(projectId, storyId, id2)).Return(task2);
                storyService.SaveTask(task2);
            }

            using (mockery.Playback()) {
                var controller = new TaskController(storyService);
                var result = controller.SignUp(initials, new[] { 
                    string.Format("{0}-{1}-{2}", projectId, storyId, id),
                    string.Format("{0}-{1}-{2}", projectId, storyId, id2),
                });
                var redirectResult = result as RedirectToRouteResult;
                Assert.NotNull(redirectResult);
                //Assert.AreEqual("Get", redirectResult.RouteName);
            }
        }

        [Test]
        public void SignUp_Lowercase_Initials() {
            var mockery = new MockRepository();

            var projectId = 3;
            var storyId = 4;
            var id = 5;
            var initials = "nn/gz";
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
                var result = controller.SignUp(initials, new[] { string.Format("{0}-{1}-{2}", projectId, storyId, id) });
                var redirectResult = result as RedirectToRouteResult;
                Assert.NotNull(redirectResult);
                Assert.True(task.Description.EndsWith("(NN/GZ)"));
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
                var result = controller.SignUp(initials, new[] { string.Format("{0}-{1}-{2}", projectId, storyId, id) });
                var redirectResult = result as RedirectToRouteResult;
                Assert.NotNull(redirectResult);
                Assert.True(task.Description.EndsWith("(NN/GZ)"));
                Assert.False(task.Description.Contains("(AA/FF)"));
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
                var result = controller.SignUp(initials, new[] { string.Format("{0}-{1}-{2}", projectId, storyId, id) });
                var viewResult = result as RedirectToRouteResult;
                Assert.NotNull(viewResult);
                Assert.True(task.Description.EndsWith("(NN/GZ)"));
                Assert.False(task.Description.Contains("AA/FF"));
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
                var result = controller.SignUp(initials, new[] { string.Format("{0}-{1}-{2}", projectId, storyId, id) });
                var viewResult = result as RedirectToRouteResult;
                Assert.NotNull(viewResult);
                Assert.False(task.Description.Contains("(AA/FF)"));
            }
        }

        [Test]
        public void Complete() {
            var mockery = new MockRepository();

            var projectId = 3;
            var storyId = 4;
            var id = 5;
            var initials = true;
            var description = "Doin work";
            var task = new Task { Description = description, Id = id, ParentStoryId = storyId, ProjectId = projectId, Complete = false };

            var storyService = mockery.StrictMock<IStoryService>();

            using (mockery.Record())
            using (mockery.Ordered()) {
                Expect.Call(storyService.GetTask(projectId, storyId, id)).Return(task);
                storyService.SaveTask(task);
            }

            using (mockery.Playback()) {
                var controller = new TaskController(storyService);
                var result = controller.Complete(id, storyId, projectId, initials);
                var viewResult = result as PartialViewResult;
                Assert.NotNull(viewResult);
                Assert.AreEqual("TaskDetails", viewResult.ViewName);
                Assert.IsInstanceOf<TaskViewModel>(viewResult.Model);
                var modelTask = viewResult.Model as TaskViewModel;
                Assert.AreEqual(projectId, modelTask.ProjectId);
                Assert.AreEqual(storyId, modelTask.StoryId);
                Assert.AreEqual(id, modelTask.Id);
                Assert.IsTrue(modelTask.Complete);
            }
        }

        [Test]
        public void Complete_Doesnt_Save_Task_If_No_Change() {
            var mockery = new MockRepository();

            var projectId = 3;
            var storyId = 4;
            var id = 5;
            var completed = true;
            var description = "Doin work";
            var task = new Task { Description = description, Id = id, ParentStoryId = storyId, ProjectId = projectId, Complete = true };

            var storyService = mockery.StrictMock<IStoryService>();

            using (mockery.Record())
            using (mockery.Ordered()) {
                Expect.Call(storyService.GetTask(projectId, storyId, id)).Return(task);
            }

            using (mockery.Playback()) {
                var controller = new TaskController(storyService);
                var result = controller.Complete(id, storyId, projectId, completed);
                var viewResult = result as PartialViewResult;
                Assert.NotNull(viewResult);
                Assert.AreEqual("TaskDetails", viewResult.ViewName);
                Assert.IsInstanceOf<TaskViewModel>(viewResult.Model);
                Assert.AreEqual(task, (viewResult.Model as TaskViewModel).Task);
            }
        }

        [Test]
        public void Reorder() {
            var mockery = new MockRepository();
            var storyService = mockery.StrictMock<IStoryService>();
            storyService.Expect(svc => svc.ReorderTasks(0, 0, null)).IgnoreArguments();
            var ids = "1-2-1,1-2-2,1-2-3";

            mockery.ReplayAll();
            var controller = new TaskController(storyService);
            var result = controller.ReOrder(ids);
            
            var emptyResult = result as EmptyResult;
            Assert.NotNull(emptyResult);
            var args = storyService.GetArgumentsForCallsMadeOn(service => service.ReorderTasks(0, 0, null));
            Assert.AreEqual(1, Convert.ToInt32(args[0][0]), "project id");
            Assert.AreEqual(2, Convert.ToInt32(args[0][1]), "story id");
            var tasks = args[0][2] as List<Task>;
            Assert.NotNull(tasks);
            Assert.AreEqual(3, tasks.Count);
            for (int i = 0; i < tasks.Count; i++) {
                var task = tasks[i];
                Assert.AreEqual(1, task.ProjectId);
                Assert.AreEqual(2, task.ParentStoryId);
                Assert.AreEqual(i + 1, task.Position);
                Assert.AreEqual(i + 1, task.Id);
            }
        }
    }
}
