using Shop.EntityFramework.Entities;
using Shop.EntityFramework.Infrastructures.Entities.Auditing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shop.EntityFramework.Infrastructures.Repository
{
    public class Repository<T> : ReadOnlyRepository<T>, IRepository<T> where T : Entity
    {
        protected readonly ICurrentPrincipal _principal;

        public Repository(DbContext context, ICurrentPrincipal principal) : base(context)
        {
            _principal = principal;
        }

        public bool Delete(T entity)
        {
            if(typeof(T).GetProperties().Any(x => x.Name == nameof(IHasDeleted.IsDeleted)))
            {
                typeof(T).GetProperties().FirstOrDefault(x => x.Name == nameof(IHasDeleted.IsDeleted)).SetValue(entity, true);
                typeof(T).GetProperties().FirstOrDefault(x => x.Name == nameof(IHasDeleted.DeletedTime)).SetValue(entity, DateTime.Now);
                typeof(T).GetProperties().FirstOrDefault(x => x.Name == nameof(IHasDeleted.DeletedUser)).SetValue(entity, _principal.CurrentUserId);
            }
            else
            {
                _dbSet.Remove(entity);
            }
            return true;
        }

        public bool DeleteMany(IEnumerable<T> entities)
        {
            return DeleteMany(entities.ToArray());
        }

        public bool DeleteMany(T[] entities)
        {
            foreach (var item in entities)
            {
                if (!Delete(item))
                    return false;
            }
            return true;
        }

        public bool DeleteMany(Func<T, bool> predicate)
        {
            var entities = GetList(predicate).ToArray();
            return DeleteMany(entities);
        }

        public T Insert(T entity)
        {
            if (entity.Id == default)
                entity.Id = Guid.NewGuid();

            if (typeof(T).GetProperties().Any(x => x.Name == nameof(IHasCreation.CreationTime)))
            {
                typeof(T).GetProperties().FirstOrDefault(x => x.Name == nameof(IHasCreation.CreationTime)).SetValue(entity, DateTime.Now);
                typeof(T).GetProperties().FirstOrDefault(x => x.Name == nameof(IHasCreation.CreationUser)).SetValue(entity, _principal.CurrentUserId);
            }
            return _dbSet.Add(entity);
        }

        public bool InsertMany(IEnumerable<T> entities)
        {
            return InsertMany(entities.ToArray());
        }

        public bool InsertMany(T[] entities)
        {
            foreach (var item in entities)
            {
                Insert(item);
            }
            return true;
        }

        public void SaveChange()
        {
            _context.SaveChanges();
        }

        public bool Update(T entity)
        {
            if (typeof(T).GetProperties().Any(x => x.Name == nameof(IHasModified.LastModifiedTime)))
            {
                typeof(T).GetProperties().FirstOrDefault(x => x.Name == nameof(IHasModified.LastModifiedTime)).SetValue(entity, DateTime.Now);
                typeof(T).GetProperties().FirstOrDefault(x => x.Name == nameof(IHasModified.LastModifiedUser)).SetValue(entity, _principal.CurrentUserId);
            }
            _context.Entry(entity).State = EntityState.Modified;
            return true;
        }

        public void Update(Category sale)
        {
            throw new NotImplementedException();
        }

        public bool UpdateMany(IEnumerable<T> entities)
        {
            return UpdateMany(entities.ToArray());
        }

        public bool UpdateMany(T[] entities)
        {
            foreach (var item in entities)
            {
                Update(item);
            }
            return true;
        }
    }
}
