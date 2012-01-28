using System;
using System.Web.Mvc;
using PivotalTrackerDotNet;
using PivotalExtension.TaskManager.Models;
using PivotalTrackerDotNet.Domain;

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
            return GetStory(storyId, projectId);
        }

        [HttpGet]
        public ActionResult BackLogStories(int projectId) {
            return View(Service.GetBacklogStories(projectId));
        }

        [HttpGet]
        public ActionResult IceboxStories(int projectId) {
            return View(Service.GetIceboxStories(projectId));
        }

        [HttpPost]
        public ActionResult Start(int projectId, int storyId) {
            var startedStory = Service.StartStory(projectId, storyId);
            return PartialView("StoryRow", new StoryRowViewModel(startedStory));
        }

        [HttpPost]
        public ActionResult Finish(int projectId, int storyId) {
            var finishedStory = Service.FinishStory(projectId, storyId);
            return PartialView("StoryRow", new StoryRowViewModel(finishedStory));
        }

        [HttpPost]
        public ActionResult AddTask(int projectId, int storyId, string details) {
            Service.AddNewTask(new Task { Description = details, ParentStoryId = storyId, ProjectId = projectId });
            return GetStory(storyId, projectId);
        }

        [HttpPost]
        public ActionResult DeleteTask(int storyId, int projectId, int taskId)
        {
            Service.RemoveTask(projectId, storyId, taskId);
            return GetStory(storyId, projectId);
        }

        private ActionResult GetStory(int storyId, int projectId)
        {
            var story = Service.GetStory(projectId, storyId);
            return PartialView("StoryRow", new StoryRowViewModel(story));
        }
    }
}