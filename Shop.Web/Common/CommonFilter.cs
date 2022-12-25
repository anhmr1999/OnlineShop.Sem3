using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Web.Common
{
    public class CommonFilter
    {
        public int PageIndex { get; set; } = 1;
        public string SearchKey { get; set; }
    }

    public class CommonListResult<T> where T : class
    {
        public int TotalCount { get; set; }
        public decimal TotalPage { get; set; }
        public CommonFilter Filter { get; set; }
        public ICollection<T> List { get; set; }
    }
}