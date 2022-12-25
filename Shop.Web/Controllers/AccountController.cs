using Newtonsoft.Json;
using Shop.EntityFramework;
using Shop.EntityFramework.Entities;
using Shop.Web.Authentications;
using Shop.EntityFramework.Common;
using Shop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Shop.Web.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View(new LoginModel());
        }

        [HttpPost]
        public ActionResult Login(LoginModel loginModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(loginModel.Username, loginModel.Password))
                {
                    var user = (ShopMembershipUser)Membership.GetUser(loginModel.Username, false);
                    if (user != null)
                    {
                        AuthenticationModel userModel = new AuthenticationModel()
                        {
                            Id = user.Id.Value,
                            Email = user.Email,
                            Name = user.Name,
                            SurName = user.SurName,
                            Permissions = user.Permissions.Select(x => x.Name).ToArray(),
                            Roles = user.Roles.Select(x => x.RoleName).ToArray()
                        };

                        string userData = JsonConvert.SerializeObject(userModel);
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                        (1, loginModel.Username, DateTime.Now, DateTime.Now.AddMinutes(30), false, userData);

                        string enTicket = FormsAuthentication.Encrypt(authTicket);
                        HttpCookie faCookie = new HttpCookie(AuthenticationCookieNameConsts.AuthenticationCookieName, enTicket);
                        Response.Cookies.Add(faCookie);
                    }

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(new LoginModel());
        }

        public ActionResult Registration()
        {
            return View(new RegistrationModel());
        }

        [HttpPost]
        public ActionResult Registration(RegistrationModel registrationModel)
        {
            bool statusRegistration = false;
            string messageRegistration = string.Empty;

            if (ModelState.IsValid)
            {
                // Email Verification
                string userName = Membership.GetUserNameByEmail(registrationModel.Username);
                if (!string.IsNullOrEmpty(userName))
                {
                    ModelState.AddModelError(nameof(RegistrationModel.Username), "Sorry: Email already exists");
                    return View(registrationModel);
                }

                using (ShopDbContext dbContext = new ShopDbContext())
                {
                    var user = new User()
                    {
                        Username = registrationModel.Username,
                        SurName = registrationModel.SurName,
                        Name = registrationModel.Name,
                        Email = registrationModel.Email,
                        Password = registrationModel.Password.ToMd5(),
                        Id = Guid.NewGuid(),
                        CreationTime = DateTime.Now
                    };

                    dbContext.Users.Add(user);
                    dbContext.SaveChanges();
                }

                messageRegistration = "Your account has been created successfully. ^_^";
                statusRegistration = true;
            }
            else
            {
                messageRegistration = "Something Wrong!";
            }
            ViewBag.Message = messageRegistration;
            ViewBag.Status = statusRegistration;

            return View(registrationModel);
        }

        public ActionResult LogOut()
        {
            HttpCookie cookie = new HttpCookie(AuthenticationCookieNameConsts.AuthenticationCookieName, "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", null);
        }
    }
}