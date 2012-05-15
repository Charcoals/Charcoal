using System.Web.Mvc;
using Charcoal.Common.Providers;
using PivotalTrackerDotNet;

namespace Charcoal.Web.Controllers {
    public class ProjectsController : BaseController
    {

        IProjectProvider projectService;
        IProjectProvider ProjectService
        {
            get { return projectService ?? (projectService = new ProjectService(Token)); }
        }

        public ProjectsController() : this(null) { }

        public ProjectsController(IProjectProvider projectService = null)
            : base()
        {
            this.projectService = projectService;
        }

        public ActionResult Index() {
            var projects = ProjectService.GetProjects();
            return View(projects);
        }

        //public ActionResult RecentActivities(int projectId) {
        //    return View(ProjectService.GetRecentActivity(projectId));
        //} 

    }
}
