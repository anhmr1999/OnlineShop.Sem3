using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.EntityFramework.Infrastructures.Entities.Auditing
{
    public class FullAuditedEntity : Entity, IHasCreation, IHasModified, IHasDeleted
    {
        public DateTime CreationTime { get; set; }
        public Guid? CreationUser { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public Guid? LastModifiedUser { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedTime { get; set; }
        public Guid? DeletedUser { get; set; }
    }
}
