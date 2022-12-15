using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.EntityFramework.Infrastructures
{
    public interface ICurrentPrincipal
    {
        Guid? CurrentUserId { get; }
        UserPrincipal CurrentUser { get; }
    }
}
