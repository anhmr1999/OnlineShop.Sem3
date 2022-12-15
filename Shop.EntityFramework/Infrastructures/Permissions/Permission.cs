using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.EntityFramework.Infrastructures.Permissions
{
    public class Permission
    {
        public Permission(string name, string displayName, string parrentName = "", ICollection<Permission> permissions = null)
        {
            Name = name;
            DisplayName = displayName;
            ParentName = parrentName;
            Permissions = permissions;
        }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string ParentName { get; set; }
        public ICollection<Permission> Permissions { get; set; }
    }
}
