using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using PivotalTrackerDotNet;
using PivotalTrackerDotNet.Domain;

namespace PivotalExtension {
	public partial class _Default : System.Web.UI.Page {
		static readonly AuthenticationToken Token = AuthenticationService.Authenticate("v5core", "changeme");
		const int ProjectId = 424921;

		protected static StoryService Service = new StoryService(Token);
		protected static List<Person> Members = new MembershipService(Token).GetMembers(ProjectId);

		protected bool HideCompletedTasks = false;

		protected void Page_Load(object sender, EventArgs e) {
			StoryRepeater.DataSource = Service.GetStories(ProjectId);
			StoryRepeater.DataBind();
		}

		protected void HideCompletedCheckbox_Click(object sender, EventArgs e) {
			HideCompletedTasks = HideCompletedCheckbox.Checked;
			StoryRepeater.DataBind();
		}

		protected void CompleteTaskLink_Click(object sender, EventArgs e) {
			var ids = ((LinkButton)sender).CommandArgument.Split(':');
		}
	}
}
