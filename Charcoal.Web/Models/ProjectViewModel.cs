using PivotalTrackerDotNet.Domain;

namespace Charcoal.Web.Models {
    public class ProjectViewModel {
        public Project Project { get; private set; }

        public ProjectViewModel(Project project) {
            Project = project;
        }

        public string StartDate {
            get { return string.Format("Starts every {0} week on {1}.", Project.IterationLength, Project.WeekStartDay); }
        }

        public string Velocity {
            get { return string.Format("{0} points", Project.CurrentVelocity); }
        }

        public string VelocityScheme {
            get { return Project.VelocityScheme; }
        }

        public string Name {
            get { return Project.Name; }
        }

        public string LatestEvent {
            get { return "Last updated on " + Project.LastActivityAt; }
        }

        public int Id
        {
            get { return Project.Id; }
        }
    }
}