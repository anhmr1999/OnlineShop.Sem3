using Shop.Web.Common.ObjectRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Web.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SaveBill(BillObject[] bills)
        {

            return View();
        }
    }
}