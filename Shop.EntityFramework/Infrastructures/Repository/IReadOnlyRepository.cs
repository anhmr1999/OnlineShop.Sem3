using Shop.EntityFramework.Infrastructures.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.EntityFramework.Infrastructures.Repository
{
    public interface IReadOnlyRepository<T> where T : Entity
    {
        IQueryable<T> GetQueryable();
        List<T> GetList(Func<T, Boolean> predicate);
        T Get(Guid id);
        bool Any(Func<T, Boolean> predicate);
    }
}
