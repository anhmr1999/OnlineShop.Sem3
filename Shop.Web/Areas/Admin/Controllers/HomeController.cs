using Shop.EntityFramework.Infrastructures.Permissions;
using Shop.Web.Authentications;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;

namespace Shop.Web.Areas.Admin.Controllers
{
    [ShopAuthorize(Proxy = PermissionName.Admin)]
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            ViewData["key"] = "ggggg";
            return View();
        }
    }
}