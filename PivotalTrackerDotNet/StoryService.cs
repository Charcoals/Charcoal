using System.Collections.Generic;
using PivotalTrackerDotNet.Domain;
using RestSharp;

namespace PivotalTrackerDotNet {
	public class StoryService : AAuthenticatedService {
		private const string StoryEndpoint = "projects/{0}/stories";
		public List<Story> CachedStories { get; private set; }

		public StoryService(AuthenticationToken token)
			: base(token) {
			CachedStories = new List<Story>();
		}

		public Story GetStory(int projectId, int storyId) {
			var request = BuildRequest();
			request.Resource = string.Format(StoryEndpoint + "/{1}", projectId, storyId);

			var response = RestClient.Execute<Story>(request);
			return response.Data;
		}

		public List<Story> GetStories(int projectId) {
			var request = BuildRequest();
			request.Resource = string.Format(StoryEndpoint, projectId);

			var response = RestClient.Execute<List<Story>>(request);
			return response.Data;
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
