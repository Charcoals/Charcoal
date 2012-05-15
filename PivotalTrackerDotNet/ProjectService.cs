using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charcoal.Common.Providers;
using PivotalTrackerDotNet.Domain;

namespace PivotalTrackerDotNet
{
    public class ProjectService : AAuthenticatedService, IProjectProvider
    {
        const string ProjectsEndpoint = "projects/";
        const string AcitivityEndpoint = "projects/{0}/activities?limit={1}";

        public ProjectService(string Token) : base(Token) { }

        public List<Activity> GetRecentActivity(int projectId)
        {
            return GetRecentActivity(projectId, 30);
        }

        public List<Activity> GetRecentActivity(int projectId, int limit)
        {
            var request = BuildGetRequest();
            request.Resource = string.Format(AcitivityEndpoint, projectId, limit);
            var response = RestClient.Execute<List<Activity>>(request);
            return response.Data;
        }

        public List<Charcoal.Common.Entities.Project> GetProjectsByUser(string userName)
        {
            throw new NotImplementedException();
        }


        public List<Charcoal.Common.Entities.Project> GetProjects()
        {
            var request = BuildGetRequest();
            request.Resource = ProjectsEndpoint;

            var response = RestClient.Execute<List<Project>>(request);
            return response.Data.ConvertAll(ConvertProject);
        }

        Charcoal.Common.Entities.Project ConvertProject(Project project)
        {
            return new Charcoal.Common.Entities.Project
                       {
                           Id = project.Id,
                           Title = project.Name,
                           Description = CreateDescription(project)
                       };
        }

        private string CreateDescription(Project project)
        {
            return "";
        }
    }
}
