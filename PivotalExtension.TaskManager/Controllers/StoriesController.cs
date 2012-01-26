using System.Web.Mvc;
using PivotalTrackerDotNet;
using PivotalExtension.TaskManager.Models;

namespace PivotalExtension.TaskManager.Controllers {
	public class StoriesController : BaseController {
		IStoryService service;
		IStoryService Service {
			get { return service ?? (service = new StoryService(Token)); }
		}
		public StoriesController() : this(null) { }

		public StoriesController(IStoryService service = null)
			: base() {
			this.service = service;
		}

		[HttpGet]
		public ActionResult CurrentIteration(int projectId) {
			return View(Service.GetCurrentStories(projectId));
		}

		[HttpGet]
		public ActionResult Get(int projectId, int storyId) {
			var story = Service.GetStory(projectId, storyId);
			return PartialView("StoryRow", new StoryRowViewModel(story));
		}

		[HttpGet]
		public ActionResult BackLogStories(int projectId) {
			return View(Service.GetBacklogStories(projectId));
		}

		[HttpGet]
		public ActionResult IceboxStories(int projectId) {
			return View(Service.GetIceboxStories(projectId));
		}
	}
}
