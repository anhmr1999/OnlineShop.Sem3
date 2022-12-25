using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Shop.Web.Common
{
    public static class CommonAction
    {
        public static string ToMd5(this string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                var hash = BitConverter.ToString(hashBytes).Replace("-", "");
                return hash;
            }
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            if (condition)
                return query.Where(predicate);
            return query;
        }

        public static IEnumerable<T> PagedBy<T>(this IEnumerable<T> query, CommonFilter filter)
        {
            var skip = (filter.PageIndex - 1) * 10;
            return query.Skip(skip).Take(10);
        }
    }
}