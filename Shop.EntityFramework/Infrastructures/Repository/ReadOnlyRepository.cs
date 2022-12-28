﻿using Shop.EntityFramework.Entities;
using Shop.EntityFramework.Infrastructures.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
            return GetQueryable().Any(predicate);
        }

        public T Get(Guid id)
        {
            return GetQueryable().FirstOrDefault(x => x.Id == id);
        }

        public List<T> GetList(Func<T, bool> predicate)
        {
            return GetQueryable().Where(predicate).ToList();
        }

        public IQueryable<T> GetQueryable()
        {
            if(typeof(T).GetProperties().Any(x => x.Name == nameof(FullAuditedEntity.IsDeleted)))
            {
                var prop = typeof(T).GetProperty(nameof(FullAuditedEntity.IsDeleted));
                var lst = _dbSet.AsQueryable().Where(PropertyEquals<T, bool>(prop, false)).ToList();
                return _dbSet.AsQueryable().Where(PropertyEquals<T, bool>(prop, false));
            }

            return _dbSet.AsQueryable();
        }

        public static Expression<Func<TItem, bool>> PropertyEquals<TItem, TValue>(PropertyInfo property, TValue value)
        {
            var param = Expression.Parameter(typeof(TItem));
            var body = Expression.Equal(Expression.Property(param, property),
                Expression.Constant(value));
            return Expression.Lambda<Func<TItem, bool>>(body, param);
        }
    }
}
