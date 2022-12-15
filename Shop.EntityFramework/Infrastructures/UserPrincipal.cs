using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Shop.EntityFramework.Infrastructures
{
    public class UserPrincipal : IPrincipal
    {
        public Guid? Id { get; set; }
        public string SurName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }
        public string[] Permissions { get; set; }

        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            return Roles.Any(r => r.Contains(role));
        }

        public bool IsHasPermission(string proxy)
        {
            return Permissions.Any(x => proxy == x);
        }

        public UserPrincipal(string username)
        {
            Identity = new GenericIdentity(username);
        }
    }
}
