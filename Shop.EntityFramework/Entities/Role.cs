using Shop.EntityFramework.Infrastructures.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.EntityFramework.Entities
{
    public class Role : FullAuditedEntity
    {
        public string RoleName { get; set; }
        public bool IsDefault { get; set; }
        public bool IsStatic { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
