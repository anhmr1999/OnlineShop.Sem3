using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Web.Areas.Admin.Controllers
{
    public class SupplierController : Controller
    {
        // GET: Admin/Supplier
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/Add
        public ActionResult Add()
        {
            return View();
        }

        // GET: Admin/Edit
        public ActionResult Edit(string name)
        {
            return View();
        }
    }
}