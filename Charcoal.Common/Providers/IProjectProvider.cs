using System.Collections.Generic;
using Charcoal.Common.Entities;

namespace Charcoal.Common.Providers
{
	public interface IProjectProvider {
		List<Project> GetProjectsByUser(string userName);
		List<Project> GetProjects();
	}
}