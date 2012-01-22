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
    public class ProjectsControllerTest {
        [Test]
        public void Index() {
            var mockery = new MockRepository();

            var projectService = mockery.StrictMock<IProjectService>();

            using (mockery.Record()) {
                Expect.Call(projectService.GetProjects()).Return(new List<Project>());
            }

            using (mockery.Playback()) {
                var controller = new ProjectsController(projectService);
                var result = controller.Index();
                var viewResult = result as ViewResult;
                Assert.NotNull(viewResult);
                Assert.IsInstanceOf<List<Project>>(viewResult.Model);
            }
        }
    }
}
