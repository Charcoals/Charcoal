using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PivotalConnect;
using PivotalTrackerDotNet;

namespace PivotalExtension {
	public partial class _Default : System.Web.UI.Page {
		protected readonly Pivotal Pivotal = Pivotal.Instance;
		protected static StoryService Service = new StoryService(AuthenticationService.Authenticate("v5core", "changeme"));
		const int ProjectId = 424921;
		
		protected bool hideCompletedTasks = false;

		protected void Page_Load(object sender, EventArgs e) {
			StoryRepeater.DataSource = Service.GetCurrentStories(ProjectId);
			StoryRepeater.DataBind();
		}

		protected void HideCompletedCheckbox_Click(object sender, EventArgs e) {
			hideCompletedTasks = HideCompletedCheckbox.Checked;
			StoryRepeater.DataBind();
		}
	}
}
