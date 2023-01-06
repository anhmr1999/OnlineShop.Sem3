using Shop.EntityFramework.Entities;
using Shop.EntityFramework.Infrastructures.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Web.Controllers
{
    public class AlbumController : Controller
    {
        private readonly IRepository<Product> _productRepository;

        public AlbumController(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }
        // GET: Album
        public ActionResult Index()
        {
            return View();
          
        }
        public ActionResult GetAll()
        {
            var sale = _productRepository.GetQueryable();
            if (sale == null)
                return RedirectToAction("Index");
            return Json(sale, JsonRequestBehavior.AllowGet);
        }
    }
}