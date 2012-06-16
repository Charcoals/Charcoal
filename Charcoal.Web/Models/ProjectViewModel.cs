using Charcoal.Common.Entities;

namespace Charcoal.Web.Models {
    public class ProjectViewModel {
        public Project Project { get; private set; }

        public ProjectViewModel(Project project) {
            Project = project;
        }


        public string Description {
            get { return Project.Description; }
        }

        public string Name {
            get { return Project.Title; }
        }


        public long Id
        {
            get { return Project.Id; }
        }

        public int Velocity
        {
            get { return Project.Velocity; }
        }
    }
}