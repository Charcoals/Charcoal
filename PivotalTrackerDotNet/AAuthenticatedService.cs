using PivotalTrackerDotNet.Domain;
using RestSharp;

namespace PivotalTrackerDotNet {
	public abstract class AAuthenticatedService {
		protected readonly AuthenticationToken m_token;
		protected RestClient RestClient;
		protected AAuthenticatedService(AuthenticationToken token) {
			m_token = token;
			RestClient = new RestClient();
			RestClient.BaseUrl = PivotalTrackerRestEndpoint.ENDPOINT;
		}

		protected RestRequest BuildRequest() {
			var request = new RestRequest(Method.GET);
			request.AddHeader("X-TrackerToken", m_token.Guid.ToString("N"));
			request.RequestFormat = DataFormat.Xml;
			return request;
		}
	}
}