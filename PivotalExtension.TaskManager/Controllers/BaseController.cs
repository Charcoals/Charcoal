using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using PivotalTrackerDotNet.Domain;

namespace PivotalExtension.TaskManager.Controllers {
    public class BaseController : Controller {
        protected AuthenticationToken Token {
            get { return (AuthenticationToken)Session["token"]; }
            set { Session.Add("token", value); }
        }
    }
}
