using System.Web.Mvc;
using Charcoal.Common.Entities;
using Charcoal.Web.Models;
using PivotalTrackerDotNet;
using Charcoal.Common.Providers;

namespace Charcoal.Web.Controllers {
	public class StoriesController : BaseController {
		IStoryProvider service;
        IStoryProvider Service
        {
			get { return service ?? (service = new StoryService(Token)); }
		}
		public StoriesController() : this(null) { }

        public StoriesController(IStoryProvider service = null)
			: base() {
			this.service = service;
		}

		[HttpGet]
		public ActionResult CurrentIteration(int projectId) {
			return View(new ProjectStoryViewModel(projectId, Service.GetStories(projectId, IterationType.Current)));
		}

		//put is needed so we can redirect here from Put requests
		[AcceptVerbs(HttpVerbs.Put | HttpVerbs.Get)]
        public ActionResult Get(int projectId, int storyId, int iterationType)
        {
            return GetStory(storyId, projectId, (IterationType)iterationType);
		}

		[HttpGet]
		public ActionResult BackLogStories(int projectId) {
            return View(new ProjectStoryViewModel(projectId, Service.GetStories(projectId, IterationType.Backlog)));
		}

		[HttpGet]
		public ActionResult IceboxStories(int projectId) {
            return View(new ProjectStoryViewModel(projectId, Service.GetStories(projectId, IterationType.Icebox)));
		}

		[HttpPut]
        public ActionResult Start(int projectId, int storyId, int iteration)
        {
            var iterationType = (IterationType)iteration;
            var startedStory = Service.StartStory(projectId, storyId, iterationType);
		    return PartialView("StoryRow", new StoryRowViewModel(startedStory, iterationType));
		}

		[HttpPut]
		public ActionResult Finish(int projectId, int storyId, int iteration) {
		    var iterationType = (IterationType) iteration;
			var finishedStory = Service.FinishStory(projectId, storyId,iterationType );
		    return PartialView("StoryRow", new StoryRowViewModel(finishedStory, iterationType));
		}

		[HttpPost]
        public ActionResult AddTask(long projectId, long storyId, string description, string initials, IterationType iterationType)
        {
			var task = new Task { Description = description, StoryId = storyId };
			task.Description = AddInitialsToDescription(task, initials,projectId, iterationType);
			Service.AddNewTask(task, projectId);
            return GetStory(storyId, projectId, iterationType);
		}

		//TODO: this is duplicated from task controller, put somewhere better
        private string AddInitialsToDescription(Task task, string initials, long projectId, IterationType iterationType)
        {
            return new TaskViewModel(task, projectId,iterationType).GetDescriptionWithoutOwners() + (string.IsNullOrEmpty(initials) ? "" : (" (" + initials.ToUpper() + ")"));
		}

		[HttpDelete]
        public ActionResult DeleteTask(int storyId, int projectId, int taskId, int iteration)
        {
			Service.RemoveTask(projectId, storyId, taskId);
			return GetStory(storyId, projectId, (IterationType)iteration);
		}

		private ActionResult GetStory(long storyId, long projectId, IterationType iterationType) {
            var story = Service.GetStory(projectId, storyId, iterationType);
			return PartialView("StoryRow", new StoryRowViewModel(story,iterationType));
		}

		[HttpPost]
        public ActionResult AddComment(int projectId, int storyId, string comment, int iteration)
        {
			Service.AddComment(projectId, storyId, comment);
			return GetStory(storyId, projectId,(IterationType)iteration);
		}
	}
}