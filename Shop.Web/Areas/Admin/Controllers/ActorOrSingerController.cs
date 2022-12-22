using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Web.Areas.Admin.Controllers
{
    public class ActorOrSingerController : Controller
    {
        // GET: Admin/ActorAndSinger
        public ActionResult Index()
        {
            return View();
        }
        // GET: Admin/ActorAndSinger/Edit
        public ActionResult Edit(string id)
        {
            return View();
        }
        // GET: Admin/ActorAndSinger/Add
        public ActionResult Add()
        {
            return View();
        }
    }
}