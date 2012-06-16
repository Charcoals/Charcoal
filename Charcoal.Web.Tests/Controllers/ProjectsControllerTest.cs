using System.Collections.Generic;
using System.Linq;
using Charcoal.Common;
using Charcoal.Common.Entities;
using Charcoal.Web.Controllers;
using Charcoal.Web.Models;
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

            var project = new Project {Id = 4};
            projectService.Setup(e => e.GetProjects()).Returns(new List<Project>{project});

            var controller = new ProjectsController(projectService.Object);
            var result = controller.Index();
            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult);
            Assert.IsInstanceOf<ProjectsContainerViewModel>(viewResult.Model);

            var model = viewResult.Model as ProjectsContainerViewModel;
            Assert.AreEqual(project.Id, model.Projects.Single().Id);
            projectService.Verify();
        }

        [Test]
        public void CanCreateProject()
        {
            var projectService = new Mock<IProjectProvider>();
            var project = new Project { Id = 4, Description = "lll", Title = "sdf",Velocity = 3};
            var projectModel = new ProjectModel { Id = project.Id, Description = project.Description, Title = project.Title, Velocity = project.Velocity };

            projectService.Setup(e => e.CreateProject(It.Is<Project>(j=> j.Id == project.Id))).Returns(new OperationResponse(true));
            var controller = new ProjectsController(projectService.Object);

            var actionResult = controller.Create(projectModel);
            var viewresult = actionResult as RedirectToRouteResult;
            Assert.IsNotNull(viewresult);
            Assert.AreEqual("Index", viewresult.RouteValues.Single().Value);
            projectService.Verify();
        }
    }
}
