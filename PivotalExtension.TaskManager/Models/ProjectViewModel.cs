using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PivotalTrackerDotNet.Domain;

namespace PivotalExtension.TaskManager.Models {
    public class ProjectViewModel {
        public Project Project { get; private set; }

        public ProjectViewModel(Project project) {
            Project = project;
        }

        public string StartDate {
            get { return string.Format("{0} every {1} week.", Project.WeekStartDay, Project.IterationLength); }
        }

        public string Velocity {
            get { return string.Format("Velocity of {0} points", Project.CurrentVelocity); }
        }

        public string VelocityScheme {
            get { return Project.VelocityScheme; }
        }

        public string Name {
            get { return Project.Name; }
        }

        public string LatestEvent {
            get { return Project.LastActivityAt; }
        }
    }
}