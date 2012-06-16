using System.Collections.Generic;
using Charcoal.Common.Entities;

namespace Charcoal.Web.Models
{
    public class ProjectsContainerViewModel
    {
        public List<Project> Projects { get; private set; }
        public BackingType BackingType { get; private set; }

        public ProjectsContainerViewModel(List<Project> projects, BackingType backingType)
        {
            Projects = projects;
            BackingType = backingType;
        }

        public int Count { get { return Projects.Count; } }
    }
}