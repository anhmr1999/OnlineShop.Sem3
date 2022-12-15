using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.EntityFramework.Infrastructures.Entities.Auditing
{
    public interface IHasDeleted
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedTime { get; set;  }
        Guid? DeletedUser { get; set;  }
    }
}
