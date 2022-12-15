using Shop.EntityFramework.Infrastructures.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.EntityFramework.Infrastructures.Repository
{
    public class ReadOnlyRepository<T> : IReadOnlyRepository<T> where T : Entity
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public ReadOnlyRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public bool Any(Func<T, bool> predicate)
        {
            return _dbSet.Any(predicate);
        }

        public T Get(Guid id)
        {
            return _dbSet.FirstOrDefault(x => x.Id == id);
        }

        public List<T> GetList(Func<T, bool> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        public IQueryable<T> GetQueryable()
        {
            return _dbSet.AsQueryable();
        }
    }
}
