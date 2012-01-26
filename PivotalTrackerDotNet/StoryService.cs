using System;
using System.Collections.Generic;
using PivotalTrackerDotNet.Domain;
using RestSharp;
using RestSharp.Contrib;

namespace PivotalTrackerDotNet {
    public class StoryService : AAuthenticatedService, IStoryService {
        const string StoryIterationEndpoint = "projects/{0}/iterations/{1}";
        const string SingleStoryEndpoint = "projects/{0}/stories/{1}";
        private const string AllStoriesEndpoint = "projects/{0}/stories";
        const string TaskEndpoint = "projects/{0}/stories/{1}/tasks";
        const string SaveStoryEndpoint = "projects/{0}/stories?story[name]={1}&story[requested_by]={2}&story[description]={3}&story[story_type]={4}";
        const string SaveTaskEndpoint = "projects/{0}/stories/{1}/tasks?task[description]={2}";
        const string SingleTaskEndpoint = "projects/{0}/stories/{1}/tasks/{2}";//projects/$PROJECT_ID/stories/$STORY_ID/tasks/$TASK_ID
    	  const string IceBoxEndpoint = "projects/{0}/stories?filter=current_state:unscheduled";

        public StoryService(AuthenticationToken token)
            : base(token) {
        }

        public List<Story> GetAllStories(int projectId) {
            var request = BuildGetRequest();
            request.Resource = string.Format(AllStoriesEndpoint, projectId);

            return GetStories(projectId, request);
        }

        public Story GetStory(int projectId, int storyId) {
            var request = BuildGetRequest();
            request.Resource = string.Format(SingleStoryEndpoint, projectId, storyId);

            var response = RestClient.Execute<Story>(request);
            var story = response.Data;

            return GetStoryWithTasks(projectId, story);
        }

        public List<Story> GetCurrentStories(int projectId) {

            return GetStoriesByIterationType(projectId, "current");
        }

        public List<Story> GetDoneStories(int projectId) {
            return GetStoriesByIterationType(projectId, "done");
        }

    	public List<Story> GetIceboxStories(int projectId) {
					var request = BuildGetRequest();
					request.Resource = string.Format(IceBoxEndpoint, projectId);

					return GetStories(projectId, request);
    	}

    	public List<Story> GetBacklogStories(int projectId) {
            return GetStoriesByIterationType(projectId, "backlog");
        }

        public Story RemoveStory(int projectId, int storyId) {
            var request = BuildDeleteRequest();
            request.Resource = string.Format(SingleStoryEndpoint, projectId, storyId);

            var response = RestClient.Execute<Story>(request);
            var story = response.Data;

            return story;
        }

        public Story AddNewStory(int projectId, Story toBeSaved) {
            var request = BuildPostRequest();
            request.Resource = string.Format(SaveStoryEndpoint, projectId, toBeSaved.Name, toBeSaved.RequestedBy, toBeSaved.Description, toBeSaved.StoryType);

            var response = RestClient.Execute<Story>(request);
            var story = response.Data;

            return story;
        }

        public void SaveTask(Task task) {
            var request = BuildPutRequest();
            request.Resource = string.Format(TaskEndpoint + "/{2}?task[description]={3}&task[complete]={4}", task.ProjectId, task.ParentStoryId, task.Id, HttpUtility.UrlEncode(task.Description), task.Complete.ToString().ToLower());
            RestClient.Execute(request);
        }

        public Task AddNewTask(Task task) {
            var request = BuildPostRequest();
            request.Resource = string.Format(SaveTaskEndpoint, task.ProjectId, task.ParentStoryId, task.Description);

            var response = RestClient.Execute<Task>(request);
            return response.Data;
        }

        public Task RemoveTask(int projectId, int storyId, int taskId) {
            var request = BuildDeleteRequest();
            request.Resource = string.Format(SingleTaskEndpoint, projectId, storyId, taskId);

            var response = RestClient.Execute<Task>(request);
            return response.Data;
        }

        public Task GetTask(int projectId, int storyId, int taskId) {
            var request = BuildGetRequest();
            request.Resource = string.Format(SingleTaskEndpoint, projectId, storyId, taskId);

            var response = RestClient.Execute<Task>(request);
            var output = response.Data;
            output.ParentStoryId = storyId;
            output.ProjectId = projectId;
            return output;
        }

        List<Story> GetStoriesByIterationType(int projectId, string iterationType) {
            var request = BuildGetRequest();
            request.Resource = string.Format(StoryIterationEndpoint, projectId, iterationType);

            return GetStories(projectId, request);
        }

        private List<Story> GetStories(int projectId, RestRequest request) {
					var response = RestClient.Execute < List<Story>>(request);
						var stories = response.Data;
						foreach (var story in stories) {
							GetStoryWithTasks(projectId, story);
						}
            return stories;
        }

        Story GetStoryWithTasks(int projectId, Story story) {
            var request = BuildGetRequest();
            request.Resource = string.Format(TaskEndpoint, projectId, story.Id);
            var taskResponse = RestClient.Execute<List<Task>>(request);
            story.Tasks = taskResponse.Data;
            if (story.Tasks != null) {
                story.Tasks.ForEach(e => {
                    e.ParentStoryId = story.Id;
                    e.ProjectId = projectId;
                });
            }
            return story;
        }
    }
}
