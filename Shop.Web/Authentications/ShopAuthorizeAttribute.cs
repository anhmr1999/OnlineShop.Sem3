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

        private string _proxy;

        public string Proxy
        {
            get
            {
                return _proxy ?? string.Empty;
            }
            set
            {
                _proxy = value;
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (CurrentUser == null)
                return false;

            if (!string.IsNullOrEmpty(Roles) && !string.IsNullOrEmpty(_proxy))
                return CurrentUser.IsInRole(Roles) && CurrentUser.IsHasPermission(_proxy);
            else if (!string.IsNullOrEmpty(Roles))
                return CurrentUser.IsInRole(Roles);
            else if (!string.IsNullOrEmpty(_proxy))
                return CurrentUser.IsHasPermission(_proxy);
            else
                return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            RedirectToRouteResult routeData = null;

            if (CurrentUser == null)
            {
                routeData = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new{ controller = "Account", action = "Login", Area = string.Empty}));
            }
            else
            {
                routeData = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Error", action = "AccessDenied", Area = string.Empty }));
            }

            filterContext.Result = routeData;
        }
    }
}