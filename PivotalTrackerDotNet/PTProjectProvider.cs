using System;
using System.Collections.Generic;
using Charcoal.Common.Providers;
using PivotalTrackerDotNet;
using PivotalTrackerDotNet.Domain;

namespace Charcoal.PivotalTracker
{
    public class PTProjectProvider : IProjectProvider
    {
        private ProjectService m_service;
        public PTProjectProvider(string token)
        {
            m_service=new ProjectService(token.ParseToken());
        }

        public List<Activity> GetRecentActivity(int projectId)
        {
            return m_service.GetRecentActivity(projectId, 30);
        }

        public List<Charcoal.Common.Entities.Project> GetProjectsByUser(string userName)
        {
            throw new NotImplementedException();
        }


        public List<Charcoal.Common.Entities.Project> GetProjects()
        {
            return m_service.GetProjects().ConvertAll(ConvertProject);
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
