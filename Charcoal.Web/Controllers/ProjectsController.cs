using System.Web.Mvc;
using Charcoal.Common.Providers;
using Charcoal.Web.Models;
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
            return View(new ProjectsContainerViewModel(projects, Backing));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ProjectModel projectModel)
        {
            if(ModelState.IsValid)
            {
                var project = projectModel.ConvertToProject();
                var response = projectService.CreateProject(project);
                if (response.HasSucceeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(projectModel);
        }
    }
}
