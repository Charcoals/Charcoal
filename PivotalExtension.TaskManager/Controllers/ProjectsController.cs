using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PivotalTrackerDotNet;

namespace PivotalExtension.TaskManager.Controllers {
    public class ProjectsController : BaseController
    {

        IProjectService projectService;
        IProjectService ProjectService {
            get { return projectService ?? (projectService = new ProjectService(Token)); }
        }

        public ProjectsController() : this(null) { }

        public ProjectsController(IProjectService projectService = null)
            : base()
        {
            this.projectService = projectService;
        }

        public ActionResult Index() {
            var projects = ProjectService.GetProjects();
            return View(projects);
        }

        public ActionResult RecentActivities(int projectId) {
            return View(ProjectService.GetRecentActivity(projectId));
        } 

    }
}
