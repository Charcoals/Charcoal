using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PivotalTrackerDotNet.Domain;
using RestSharp;
using RestSharp.Authenticators;

namespace PivotalTrackerDotNet {
	public class StoryService : AAuthenticatedService {
		private const string StoryEndpoint = "projects/{0}/iterations/current";
		const string TaskEndpoint = "projects/{0}/stories/{1}/tasks";
		public List<Story> CachedStories { get; private set; }

		public StoryService(AuthenticationToken token)
			: base(token) {
			CachedStories = new List<Story>();
		}

		public Story GetStory(int projectId, int storyId) {
			var request = BuildRequest();
			request.Resource = string.Format(StoryEndpoint + "/{1}", projectId, storyId);

			var response = RestClient.Execute<Story>(request);
			var story = response.Data;

			return GetStoryWithTasks(projectId, story);
		}

		public List<Story> GetStories(int projectId) {
			var request = BuildRequest();
			request.Resource = string.Format(StoryEndpoint, projectId);

			var response = RestClient.Execute<List<Story>>(request);
			var stories = response.Data;
			foreach (var story in stories) {
				GetStoryWithTasks(projectId, story);
			}
			return stories;
		}

		Story GetStoryWithTasks(int projectId, Story story) {
			var request= BuildRequest();
			request.Resource = string.Format(TaskEndpoint, projectId, story.Id);
			var taskResponse = RestClient.Execute<List<Task>>(request);
			story.Tasks = taskResponse.Data;
			if (story.Tasks != null) {
				story.Tasks.ForEach(e => e.ParentStoryId = story.Id);
			}
			return story;
		}

		RestRequest BuildRequest() {
			var request = new RestRequest(Method.GET);
			request.AddHeader("X-TrackerToken", m_token.Guid.ToString("N"));
			request.RequestFormat = DataFormat.Xml;
			return request;
		}
	}

	public abstract class AAuthenticatedService {
		protected readonly AuthenticationToken m_token;
		protected RestClient RestClient;
		protected AAuthenticatedService(AuthenticationToken token) {
			m_token = token;
			RestClient = new RestClient();
			RestClient.BaseUrl = PivotalTrackerRestEndpoint.ENDPOINT;
		}
	}
}
