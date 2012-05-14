using System.Collections.Generic;
using Charcoal.Core.Entities;

namespace Charcoal.Core.Services
{
	public interface IProjectService {
		List<Project> GetProjectsByUser(string userName);
		List<Project> GetProjects();
	}
}