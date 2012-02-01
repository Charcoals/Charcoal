using System;
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

		[HttpPost]
		public ActionResult SignUp(int id, int storyId, int projectId, string initials) {
			var task = Service.GetTask(projectId, storyId, id);
			task.Description = AddInitialsToDescription(task, initials);
			Service.SaveTask(task);
			return PartialView("TaskDetails", new TaskViewModel(task));
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
