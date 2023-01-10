using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Web.Common.PageFilter
{

    public class ViewDetailt<TSource, TLikeData> where TSource : class where TLikeData : class
    {
        public TSource Data { get; set; }
        public ICollection<TLikeData> LikeData { get; set; }
    }
    public class ViewDetailt<TSource> : ViewDetailt<TSource, TSource> where TSource : class
    { }
}