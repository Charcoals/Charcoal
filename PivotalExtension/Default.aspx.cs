using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PivotalConnect;

namespace PivotalExtension {
	public partial class _Default : System.Web.UI.Page {
		Pivotal pivotal = Pivotal.Instance;
		
		protected bool hideCompletedTasks = false;

		protected void Page_Load(object sender, EventArgs e) {
			StoryRepeater.DataSource = pivotal.GetStories();
			StoryRepeater.DataBind();
		}

		protected void HideCompletedCheckbox_Click(object sender, EventArgs e) {
			hideCompletedTasks = HideCompletedCheckbox.Checked;
			StoryRepeater.DataBind();
		}
	}
}
