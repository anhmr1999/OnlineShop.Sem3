using Shop.EntityFramework.Entities;
using Shop.EntityFramework.Infrastructures.Repository;
using Shop.Web.Common.ObjectRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shop.EntityFramework.Infrastructures.Enums;
using Shop.Web.Common;
using System.Data.Entity;

namespace Shop.Web.Areas.Admin.Controllers
{

    public class SaleController : Controller
    {
        private readonly IRepository<Sale> _saleRepository;
        private readonly IRepository<Product> _productRepotory;

        public SaleController(IRepository<Sale> saleRepository, IRepository<Product> productRepotory)
        {
            _saleRepository = saleRepository;
            _productRepotory = productRepotory;
        }
        // GET: Admin/Sale
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Edit()
        {
            return View();
        }
        public ActionResult Add()
        {
            ViewBag.Products = _productRepotory.GetQueryable().ToList();
            return View(new Sale());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Sale saleDto)
        {
            if (!ModelState.IsValid)
                return View(saleDto);
            _saleRepository.Insert(saleDto);
            _saleRepository.SaveChange();
            return RedirectToAction("Index");
        }
    }
}