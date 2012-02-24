using System;
using System.Linq;
using System.Web.Mvc;
using PivotalTrackerDotNet;
using PivotalTrackerDotNet.Domain;
using PivotalExtension.TaskManager.Models;

namespace PivotalExtension.TaskManager.Controllers {
	public class TaskController : BaseController {
		IStoryService service;
		IStoryService Service {
			get { return service ?? (service = new StoryService(Token)); }
		}
		public TaskController() : this(null) { }

		public TaskController(IStoryService service = null)
			: base() {
			this.service = service;
		}

		public ActionResult Details(int id, int storyId, int projectId) {
			return PartialView("TaskDetails", new TaskViewModel(Service.GetTask(projectId, storyId, id)));
		}

        //[HttpPost]
        //public ActionResult SignUp(int id, int storyId, int projectId, string initials) {
        //    var task = Service.GetTask(projectId, storyId, id);
        //    task.Description = AddInitialsToDescription(task, initials);
        //    Service.SaveTask(task);
        //    return PartialView("TaskDetails", new TaskViewModel(task));
        //}

        [HttpPost]
        public ActionResult SignUp(string initials, string[] fullIds) {
            int storyId = 0, taskId = 0, projectId = 0;

            foreach (var s in fullIds) {
                var parts = s.Split('-');
                if (parts.Length != 3) {
                    throw new InvalidOperationException("Must pass ids in the format projectId-storyId-taskId");
                }
                if (!int.TryParse(parts[0], out projectId)) {
                    throw new InvalidOperationException("Invalid project id");
                }
                if (!int.TryParse(parts[1], out storyId)) {
                    throw new InvalidOperationException("Invalid story id");
                }
                if (!int.TryParse(parts[2], out taskId)) {
                    throw new InvalidOperationException("Invalid task id");
                }

                var task = Service.GetTask(projectId, storyId, taskId);
                task.Description = AddInitialsToDescription(task, initials);
                Service.SaveTask(task);
            }
            return RedirectToAction("Get", "Stories", new { projectId = projectId, storyId = storyId });
        }

		private string AddInitialsToDescription(Task task, string initials) {
			return new TaskViewModel(task).GetDescriptionWithoutOwners() + (string.IsNullOrEmpty(initials) ? "" : (" (" + initials.ToUpper() + ")"));
		}

		[HttpPost]
		public ActionResult Complete(int id, int storyId, int projectId, bool completed) {
			var task = Service.GetTask(projectId, storyId, id);
			if (task.Complete != completed) {
				task.Complete = completed;
				Service.SaveTask(task);
			}
			return PartialView("TaskDetails", new TaskViewModel(task));
		}

        [HttpGet]
        public ActionResult Add(int storyId, int projectId) {
            return PartialView(new TaskViewModel(new Task { ParentStoryId = storyId, ProjectId = projectId }));
        }
	}
}
