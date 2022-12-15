using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.EntityFramework.Infrastructures.Enums
{
    public enum BillStatusEnum
    {
        Created,
        Processing,
        Shipping,
        Done,
        Cancel
    }
}
