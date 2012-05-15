using System.Collections.Generic;
using Charcoal.Common.Entities;
using Charcoal.Web.Controllers;
using NUnit.Framework;
using Rhino.Mocks;
using System.Web.Mvc;
using Charcoal.Common.Providers;

namespace Charcoal.Web.Tests.Controllers {
    [TestFixture]
    public class ProjectsControllerTest {
        [Test]
        public void Index() {
            var mockery = new MockRepository();

            var projectService = mockery.StrictMock<IProjectProvider>();

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
