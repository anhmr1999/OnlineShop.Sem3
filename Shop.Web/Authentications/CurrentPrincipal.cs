using Shop.EntityFramework.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Web.Authentications
{
    public class CurrentPrincipal : ICurrentPrincipal
    {
        public Guid? CurrentUserId => CurrentUser?.Id;

        public UserPrincipal CurrentUser => HttpContext.Current.User as UserPrincipal;
    }
}