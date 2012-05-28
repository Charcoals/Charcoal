using System;
using System.Collections.Generic;
using Charcoal.Common.Entities;
using Charcoal.Common.Providers;
using Charcoal.DataLayer;

namespace Charcoal.Core
{
    public class CharcoalProjectProvider: IProjectProvider
    {
        private readonly string m_token;
        private readonly IProjectRepository m_projectRepository;

        public CharcoalProjectProvider(string token, IProjectRepository projectRepository=null)
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
            return m_projectRepository.GetProjectsByUseToken(m_token);
        }
    }
}