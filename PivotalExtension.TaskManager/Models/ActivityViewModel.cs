using System.Globalization;
using PivotalTrackerDotNet.Domain;

namespace PivotalExtension.TaskManager.Models {
    public class ActivityViewModel {
        public ActivityViewModel(Activity activity) {
            Activity = activity;
		}

        public Activity Activity { get; private set; }

        public string PublicationDate
        {
            get { return Activity.OccurredAt; }
        }

        public string GetStyle()
        {
            return "";
        }

        public string GetIcon()
        {
            string icon = "/Content/";
            if (Activity.EventType.EndsWith("update"))
                icon += "edit.png";
            else if (Activity.EventType.EndsWith("delete"))
                icon += "delete.gif";
            else if (Activity.EventType.EndsWith("create"))
                icon += "new.jpg";

            return string.Format("<img src=\"{0}\" />", icon);
        }
    }
}