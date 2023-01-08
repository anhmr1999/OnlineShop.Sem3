using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Web.Common.PageFilter
{
    public class ViewDetailt<TSource> where TSource : class
    {
        public TSource Data { get; set; }
        public ICollection<TSource> LikeData { get; set; }
    }
}