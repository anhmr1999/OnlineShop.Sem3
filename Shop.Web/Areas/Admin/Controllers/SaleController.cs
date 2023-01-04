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
        private readonly IRepository<Album> _album;

        public SaleController(IRepository<Sale> saleRepository, IRepository<Album> album)
        {
            _saleRepository = saleRepository;
            _album = album;
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