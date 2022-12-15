using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.EntityFramework.Infrastructures.Entities.Auditing
{
    public interface IHasModified
    {
        DateTime? LastModifiedTime { get; set; }
        Guid? LastModifiedUser { get; set; }
    }
}
