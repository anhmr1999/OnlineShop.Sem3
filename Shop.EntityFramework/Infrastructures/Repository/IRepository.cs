using Shop.EntityFramework.Infrastructures.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.EntityFramework.Infrastructures.Repository
{
    public interface IRepository<T> : IReadOnlyRepository<T> where T : Entity
    {
        T Insert(T entity);
        bool InsertMany(IEnumerable<T> entities);
        bool InsertMany(T[] entities);
        
        bool Update(T entity);
        bool UpdateMany(IEnumerable<T> entities);
        bool UpdateMany(T[] entities);

        bool Delete(T entity);
        bool DeleteMany(IEnumerable<T> entities);
        bool DeleteMany(T[] entities);
        bool DeleteMany(Func<T, Boolean> predicate);
    }
}
