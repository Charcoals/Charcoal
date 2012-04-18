﻿using System.Web.Mvc;
using System.Web.Security;
using Charcoal.Web.Models;
using PivotalTrackerDotNet;

namespace Charcoal.Web.Controllers {
    [SessionState(System.Web.SessionState.SessionStateBehavior.Required)]
    public class AccountController : BaseController {

        //
        // GET: /Account/LogOn

        public ActionResult LogOn() {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = 999;
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl) {
            if (ModelState.IsValid) {
                var token = AuthenticationService.Authenticate(model.UserName, model.Password);
                if (token != null) {
                    Token = token;
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\")) {
                        return Redirect(returnUrl);
                    }
                    else {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff() {
            FormsAuthentication.SignOut();
            Token = null;
            return RedirectToAction("Index", "Home");
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus) {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus) {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
