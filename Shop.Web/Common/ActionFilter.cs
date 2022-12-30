using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Web.Common
{
    public class CategoryAdminFilter : CommonFilter
    {
        public bool? CateFor { get; set; }
    }

    public class UserAdminFilter : CommonFilter
    {
        public bool? IsAdmin { get; set; }
    }

    public class SongTrailerGameAdminFilter : CommonFilter
    {
        public Guid? CategoryId { get; set; }
        public Guid? ProducerId { get; set; }
        public bool? AllowDownloadFree { get; set; }
    }
}