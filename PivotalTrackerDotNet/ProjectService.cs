using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PivotalTrackerDotNet.Domain;

namespace PivotalTrackerDotNet {
    public class ProjectService : AAuthenticatedService, IProjectService {
        const string projectsEndpoint = "projects/";

        public ProjectService(AuthenticationToken Token) : base(Token) { }

        public List<Project> GetProjects() {
            var request = BuildGetRequest();
            request.Resource = projectsEndpoint;

            var response = RestClient.Execute<List<Project>>(request);
            return response.Data;
        }
    }
}
