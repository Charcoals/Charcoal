using System;
using System.Web.Mvc;
using PivotalTrackerDotNet;
using PivotalTrackerDotNet.Domain;

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

        [HttpPost]
        public ActionResult SignUp(int id, int storyId, int projectId, string initials) {
            var task = Service.GetTask(projectId, storyId, id);
            task.Description = task.GetDescriptionWithoutOwners() + (string.IsNullOrEmpty(initials) ? "" : (" (" + initials + ")"));
            Service.SaveTask(task);
            return PartialView("TaskDetails", task);
        }

    }
}
