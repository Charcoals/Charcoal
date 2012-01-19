using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PivotalTrackerDotNet.Domain;

namespace PivotalTrackerDotNet {
	public class MembershipService : AAuthenticatedService {
		const string MemberShipEndpoint = "projects/{0}/memberships";
		public MembershipService(AuthenticationToken token)
			: base(token) {
		}

		public List<Person> GetMembers(int projectId) {
			var request = BuildGetRequest();
			request.Resource = string.Format(MemberShipEndpoint, projectId);
			var response = RestClient.Execute<List<Person>>(request);
			return response.Data;
		}
	}
}
