using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PivotalTrackerDotNet;

namespace PivotalExtension.TaskManager.Controllers {
    public class ProjectsController : BaseController {
        IProjectService service;
        IProjectService Service {
            get { return service ?? (service = new ProjectService(Token)); }
        }
        public ProjectsController() : this(null) { }

        public ProjectsController(IProjectService service = null)
            : base() {
            this.service = service;
        }

        public ActionResult Index() {
            var projects = Service.GetProjects();
            return View(projects);
        }

    }
}
