using System.Web.Mvc;
using Charcoal.Common.Providers;

namespace Charcoal.Web.Controllers {
    public class ProjectsController : BaseController
    {

        IProjectProvider projectService;

        public ProjectsController(IProjectProvider projectService)
            : base()
        {
            this.projectService = projectService;
        }

        public ActionResult Index() {
            var projects = projectService.GetProjects();
            return View(projects);
        }

    }
}
