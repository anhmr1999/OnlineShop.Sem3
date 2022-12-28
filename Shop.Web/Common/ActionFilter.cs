﻿using System;
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
}