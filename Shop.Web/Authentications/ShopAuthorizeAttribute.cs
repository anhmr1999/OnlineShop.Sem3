using Shop.EntityFramework.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Web.Authentications
{
    public class ShopAuthorizeAttribute : AuthorizeAttribute
    {
        protected UserPrincipal CurrentUser => HttpContext.Current.User as UserPrincipal;

        public string Proxy { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (CurrentUser == null)
                return false;

            if (!string.IsNullOrEmpty(Roles) && !string.IsNullOrEmpty(Proxy))
                return CurrentUser.IsInRole(Roles) && CurrentUser.IsHasPermission(Proxy);
            else if (!string.IsNullOrEmpty(Roles))
                return CurrentUser.IsInRole(Roles);
            else if (!string.IsNullOrEmpty(Proxy))
                return CurrentUser.IsHasPermission(Proxy);
            else
                return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            RedirectToRouteResult routeData = null;

            if (CurrentUser == null)
            {
                routeData = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new{ controller = "Account", action = "Login",}));
            }
            else
            {
                routeData = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
            }

            filterContext.Result = routeData;
        }
    }
}