using System;
using System.Collections.Generic;
using Charcoal.Common;
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

        public OperationResponse CreateProject(Common.Entities.Project project)
        {
            throw new NotImplementedException();
        }

        Charcoal.Common.Entities.Project ConvertProject(Project project)
        {
            return new Charcoal.Common.Entities.Project
                       {
                           Id = project.Id,
                           Title = project.Name,
                           Description = CreateDescription(project),
                           Velocity = project.CurrentVelocity
                       };
        }

        private string CreateDescription(Project project)
        {
            
            return string.Format("This project's iteration Length is of {0} weeks, and starts every {0} {1}. The last recorded activity was on {2}", project.IterationLength,project.WeekStartDay, project.LastActivityAt);
        }
    }
}
