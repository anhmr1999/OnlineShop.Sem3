using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Web.Controllers
{
    public class MusicController : Controller
    {
        // GET: Music
        public ActionResult Index()
        {
            return View();
        }

        // GET: Music Detail
        public ActionResult Views(string name)
        {
            return View();
        }
    }
}