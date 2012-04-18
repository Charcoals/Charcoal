﻿using System;
using System.Linq;
using System.Web.Mvc;
using Charcoal.Web.Models;
using PivotalTrackerDotNet;
using PivotalTrackerDotNet.Domain;

namespace Charcoal.Web.Controllers {
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

        [HttpGet]
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

        [HttpPut]
        public ActionResult SignUp(string initials, string[] fullIds) {
            int projectId = 0, storyId = 0;

            foreach(var s in fullIds) {
                int taskId = 0;
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

        [HttpPut]
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

        [HttpPut]
        public ActionResult ReOrder(string taskArray) {
            var arr = taskArray.Split(',');
            var tasks = arr.Select(ExtractTask).ToList();
            var firstTask = tasks.First();
            Service.ReorderTasks(firstTask.ProjectId, firstTask.ParentStoryId, tasks);
            return new EmptyResult();
        }

        private Task ExtractTask(string taskIds, int position) {
            var ids = taskIds.Split('-');
            return new Task { ProjectId = int.Parse(ids[0]), ParentStoryId = int.Parse(ids[1]), Id = int.Parse(ids[2]), Position = position + 1 };
        }
    }
}
