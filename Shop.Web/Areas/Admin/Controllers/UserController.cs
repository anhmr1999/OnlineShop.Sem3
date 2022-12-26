using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Web.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        // GET: Admin/User
        public ActionResult Index()
        {
            return View();
        }
        // GET: Admin/User/Add
        public ActionResult Add()
        {
            return View();
        }
        // GET: Admin/User/Edit
        public ActionResult Edit()
        {
            return View();
        }
    }
}