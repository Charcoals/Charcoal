using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PivotalTrackerDotNet;

namespace PivotalExtension.TaskManager.Controllers {
    public class StoriesController : BaseController {
        IStoryService service;
        IStoryService Service {
            get {
                if (service == null) {
                    //session data not available in ctor
                    service = new StoryService(Token);
                }
                return service;
            }
        }
        public StoriesController() : this(null) { }

        public StoriesController(IStoryService service = null)
            : base() {
            this.service = service;
        }

        public ActionResult CurrentIteration(int projectId) {
            return View(Service.GetStories(projectId));
        }

    }
}
