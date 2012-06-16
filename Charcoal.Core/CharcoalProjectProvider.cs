using System;
using System.Collections.Generic;
using Charcoal.Common;
using Charcoal.Common.Entities;
using Charcoal.Common.Providers;
using Charcoal.DataLayer;

namespace Charcoal.Core
{
    public class CharcoalProjectProvider : IProjectProvider
    {
        private readonly string m_token;
        private readonly IProjectRepository m_projectRepository;

        public CharcoalProjectProvider(string token, IProjectRepository projectRepository = null)
        {
            m_token = token;
            m_projectRepository = projectRepository ?? new ProjectRepository();
        }

        public List<Project> GetProjectsByUser(string userName)
        {
            throw new NotImplementedException();
        }

        public List<Project> GetProjects()
        {
            var projects = m_projectRepository.GetProjectsByUseToken(m_token);
            return projects.Count > 0 ? projects.ConvertAll(e => (Project) e) : new List<Project>();
        }

        public OperationResponse CreateProject(Project project)
        {
            var response= m_projectRepository.CreateProjectAssociatedWithKey(project, m_token);
            return new OperationResponse(response.HasSucceeded, response.Description);
        }
    }
}