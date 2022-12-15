using Shop.EntityFramework.Infrastructures.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.EntityFramework.Entities
{
    public class PermissionGrant : Entity
    {
        public string Name { get; set; }
        public string ProviderName { get; set; } // R - U
        public string ProviderKey { get; set; } // RoleId - UserId
    }
}
