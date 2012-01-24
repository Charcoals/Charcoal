using System.Collections.Generic;
using PivotalTrackerDotNet.Domain;

namespace PivotalTrackerDotNet {
    public class ActivityService : AAuthenticatedService, IActivityService
    {
        const string AcitivityEndpoint = "projects/{0}/activities?limit={1}";

        public ActivityService(AuthenticationToken token)
            : base(token) {
        }

        public List<Activity> GetRecentActivity(int projectId) {
            return GetRecentActivity(projectId, 30);
        }

        public List<Activity> GetRecentActivity(int projectId, int limit) {
            var request = BuildGetRequest();
            request.Resource = string.Format(AcitivityEndpoint, projectId, limit);
            var response = RestClient.Execute<List<Activity>>(request);
            return response.Data;
        }
    }
}
