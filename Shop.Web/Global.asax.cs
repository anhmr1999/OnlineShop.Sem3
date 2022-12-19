using Newtonsoft.Json;
using Shop.EntityFramework.Infrastructures;
using Shop.Web.Authentications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace Shop.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            Bootstrapper.Initialise();
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[AuthenticationCookieNameConsts.AuthenticationCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                var serializeModel = JsonConvert.DeserializeObject<AuthenticationModel>(authTicket.UserData);
                UserPrincipal principal = new UserPrincipal(authTicket.Name);
                principal.Id = serializeModel.Id;
                principal.SurName = serializeModel.SurName;
                principal.Name = serializeModel.Name;
                principal.Email = serializeModel.Email;
                principal.Roles = serializeModel.Roles;
                principal.Permissions = serializeModel.Permissions;

                HttpContext.Current.User = principal;
            }
        }
    }
}
