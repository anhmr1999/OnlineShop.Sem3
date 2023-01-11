using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Shop.Web.Common.PageFilter
{
    public class CommonPageFilter : CommonFilter
    {
        public string Order { get; set; }
    }

    public class CommonPageResult<TSource> where TSource : class
    {
        public int TotalPage { get; set; }
        public ICollection<TSource> Data { get; set; }
        public CommonPageFilter Filter { get; set; }

    }
}