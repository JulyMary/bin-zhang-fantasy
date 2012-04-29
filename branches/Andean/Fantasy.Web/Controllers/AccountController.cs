using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Fantasy.Web.Models;
using Fantasy.BusinessEngine.Services;
using Fantasy.Web.Properties;
using Fantasy.BusinessEngine;

namespace Fantasy.Web.Controllers
{

    [Authorize]
    public class AccountController : Controller
    {

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                IBusinessUserRoleService urs = BusinessEngineContext.Current.GetRequiredService<IBusinessUserRoleService>();
                BusinessUser user = urs.UserByName(model.UserName);

                if (user != null && urs.VerifyPasswod(user, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    user.OnSignIn();
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return Redirect("~/");
                    }
                }
                else
                {
                    ModelState.AddModelError("", Resources.AuthenticationFailedMessage);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            BusinessEngineContext.Current.User.OnSignIn();

            FormsAuthentication.SignOut();
            

            return Redirect("~/");
        }

      
        // GET: /Account/ChangePassword

        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    IBusinessUserRoleService urs = BusinessEngineContext.Current.GetRequiredService<IBusinessUserRoleService>();
                    BusinessUser user = BusinessEngineContext.Current.User;

                    urs.SetPassword(user, model.NewPassword);
                    changePasswordSucceeded = true;
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

    }
}
