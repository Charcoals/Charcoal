using System;
using System.Web.Mvc;
using PivotalTrackerDotNet;

namespace PivotalExtension.TaskManager.Controllers {
    public class TaskController : BaseController {
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
        public TaskController() : this(null) { }

        public TaskController(IStoryService service = null)
            : base() {
            this.service = service;
        }

        public ActionResult Details(int id, int storyId, int projectId) {
            return PartialView("TaskDetails", Service.GetTask(projectId, storyId, id));
        }

    }
}
