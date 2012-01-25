using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public ActionResult CurrentIteration(int projectId) {
            return View(Service.GetCurrentStories(projectId));
        }

        [HttpPost]
        public ActionResult RefreshStory(int projectId, int storyId)
        {
            var story = Service.GetStory(projectId, storyId);
            return PartialView("StoryRow", new StoryRowViewModel(story));
        }
    }
}
