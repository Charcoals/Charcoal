using System;
using System.Web.Mvc;
using PivotalTrackerDotNet.Domain;
using System.Web.Security;

namespace PivotalExtension.TaskManager.Controllers {
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class BaseController : Controller {
        protected AuthenticationToken Token {
            get {
                if (Session != null) {
                    var token = Session["token"];
                    if (token is AuthenticationToken) {
                        return token as AuthenticationToken;
                    }
                }
                throw new NotAuthenticatedException();
            }
            set { Session.Add("token", value); }
        }

        protected override void OnException(ExceptionContext filterContext) {
            if (filterContext.Exception is NotAuthenticatedException) {
                FormsAuthentication.SignOut();
                Response.Redirect("~/Account/LogOn?returnUrl=" + filterContext.HttpContext.Request.Url);
            }
            base.OnException(filterContext);
        }
    }

    public class NotAuthenticatedException : Exception { }
}
