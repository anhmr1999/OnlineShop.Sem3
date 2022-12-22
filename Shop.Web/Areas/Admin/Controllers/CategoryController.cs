using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Web.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Admin/Category
        public ActionResult Index()
        {
            return View();
        }
        // GET: Admin/Category/Add
        public ActionResult Add()
        {
            return View();
        }
        // GET: Admin/Category/Edit
        public ActionResult Edit()
        {
            return View();
        }
    }
}