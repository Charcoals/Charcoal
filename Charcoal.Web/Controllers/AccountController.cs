using System.Web.Mvc;
using System.Web.Security;
using Charcoal.Web.Models;
using Charcoal.Common.Providers;


namespace Charcoal.Web.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.Required)]
    public class AccountController : BaseController
    {
        private readonly IAccountProvider _accountProvider;
        
        public AccountController(IAccountProvider accountProvider)
        {
            _accountProvider = accountProvider;
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, model.FirstName, model.LastName, false, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = 999;
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                Authentication = model.Authentication;
                var token = _accountProvider.Authenticate(model.UserName, model.Password);
                if (!string.IsNullOrWhiteSpace(token))
                {
                    Token = token;
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Token = null;
            return RedirectToAction("Index", "Home");
        }
    }
}
