using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Web.Areas.Admin.Controllers
{
    public class AlbumController : Controller
    {
        // GET: Admin/Album
        public ActionResult Index()
        {
            return View();
        }
        // GET: Admin/Album/add
        public ActionResult Add()
        {
            return View();
        }
        // GET: Admin/Album/edit
        public ActionResult Edit()
        {
            return View();
        }
    }
}