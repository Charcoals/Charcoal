using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PivotalTrackerDotNet.Domain;

namespace PivotalExtension.TaskManager.Models {
	public class ProjectStoryViewModel {
		private readonly int projectId;

		public int ProjectId {
			get { return projectId; }
		}

		public IEnumerable<Story> Stories { get; private set; }

		public ProjectStoryViewModel(int projectId, IEnumerable<Story> stories) {
			this.projectId = projectId;
			Stories = stories;
		}
	}
}