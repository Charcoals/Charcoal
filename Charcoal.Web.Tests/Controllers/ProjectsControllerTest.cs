using System.Collections.Generic;
using Charcoal.Common.Entities;
using Charcoal.Web.Controllers;
using NUnit.Framework;
using System.Web.Mvc;
using Charcoal.Common.Providers;
using Moq;

namespace Charcoal.Web.Tests.Controllers
{
    [TestFixture]
    public class ProjectsControllerTest
    {
        [Test]
        public void Index()
        {

            var projectService = new Mock<IProjectProvider>();

            projectService.Setup(e => e.GetProjects()).Returns(new List<Project>());

            var controller = new ProjectsController(projectService.Object);
            var result = controller.Index();
            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult);
            Assert.IsInstanceOf<List<Project>>(viewResult.Model);
        }
    }
}
