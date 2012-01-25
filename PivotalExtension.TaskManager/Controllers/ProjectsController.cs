using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PivotalTrackerDotNet;

namespace PivotalExtension.TaskManager.Controllers {
    public class ProjectsController : BaseController
    {
        private IActivityService activityService;

        IProjectService projectService;
        IProjectService ProjectService {
            get { return projectService ?? (projectService = new ProjectService(Token)); }
        }

        public IActivityService ActivityService
        {
            get { return activityService??(activityService=new ActivityService(Token)); }
        }

        public ProjectsController() : this(null,null) { }

        public ProjectsController(IProjectService projectService = null, IActivityService activityService=null)
            : base()
        {
            this.activityService = activityService;
            this.projectService = projectService;
        }

        public ActionResult Index() {
            var projects = ProjectService.GetProjects();
            return View(projects);
        }

        public ActionResult RecentActivities(int projectId) {
            return View(ActivityService.GetRecentActivity(projectId));
        } 

    }
}
