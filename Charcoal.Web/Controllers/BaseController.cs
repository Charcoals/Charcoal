using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using Charcoal.Web.Models;

namespace Charcoal.Web.Controllers {
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class BaseController : Controller {
        protected string Token {
            get {
                if (Session != null) {
                    var token = Session["token"];
                    if (token is string) {
                        return token as string;
                    }
                }
                throw new NotAuthenticatedException();
            }
            set { Session.Add("token", value); }
        }

        protected BackingType Backing
        {
            get
            {
                var backing = ConfigurationManager.AppSettings["backingType"];
                                                             BackingType type;
                return Enum.TryParse(backing, true, out type) ? type : BackingType.Charcoal;
            }
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
