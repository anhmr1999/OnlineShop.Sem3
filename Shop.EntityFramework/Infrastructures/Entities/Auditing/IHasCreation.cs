using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.EntityFramework.Infrastructures.Entities.Auditing
{
    public interface IHasCreation
    {
        DateTime CreationTime { get; set; }
        Guid? CreationUser { get; set; }
    }
}