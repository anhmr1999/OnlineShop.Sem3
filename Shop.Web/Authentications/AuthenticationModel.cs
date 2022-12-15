using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Web.Authentications
{
    public class AuthenticationModel
    {
        public Guid Id { get; set; }
        public string SurName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }
        public string[] Permissions { get; set; }
    }
}