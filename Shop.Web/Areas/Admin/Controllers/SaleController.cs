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
using Shop.EntityFramework.Infrastructures.Permissions;
using Shop.Web.Authentications;

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
        [ShopAuthorize(Proxy = PermissionName.Sale)]
        public ActionResult Index(CommonFilter filter)
        {
            var query = _saleRepository.GetQueryable()
               .WhereIf(!string.IsNullOrEmpty(filter.SearchKey), x => x.Content.ToLower().Contains(filter.SearchKey));
            var model = new CommonListResult<Sale>();
            model.Filter = filter;
            model.TotalCount = query.Count();
            model.TotalPage = Math.Ceiling((decimal)query.Count() / 10);
            model.List = query.OrderByDescending(x => x.CreationTime).PagedBy(filter).ToList();
            return View(model);
        }
        // Edit
        [ShopAuthorize(Proxy = PermissionName.SaleEdit)]
        public ActionResult Edit(Guid id)
        {
            ViewBag.Products = _productRepotory.GetQueryable().ToList();
            var sale = _saleRepository.Get(id);
            if (sale == null)
                return RedirectToAction("Index");
            return View(sale);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ShopAuthorize(Proxy = PermissionName.SaleEdit)]
        public ActionResult Edit(Guid id, Sale saleDto)
        {
            if (!ModelState.IsValid)
                return View(saleDto);
            var sale = _saleRepository.Get(id);
            sale.StartDate = saleDto.StartDate;
            sale.EndDate = saleDto.EndDate;
            sale.Content = saleDto.Content;
            sale.Quantity = saleDto.Quantity;
            sale.Percent = saleDto.Percent;
            sale.Price = saleDto.Price;
            sale.Products = saleDto.Products;
            _saleRepository.Update(sale);
            _saleRepository.SaveChange();
            return RedirectToAction("Index");
        }
        //POST:Add Sale
        [ShopAuthorize(Proxy = PermissionName.SaleAdd)]
        public ActionResult Add()
        {
            ViewBag.Products = _productRepotory.GetQueryable().ToList();
            return View(new Sale());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ShopAuthorize(Proxy = PermissionName.SaleAdd)]
        public ActionResult Add(Sale saleDto)
        {
            if (!ModelState.IsValid)
                return View(saleDto);
            _saleRepository.Insert(saleDto);
            _saleRepository.SaveChange();
            return RedirectToAction("Index");
        }
        //DELETE: 
        [ShopAuthorize(Proxy = PermissionName.SaleDelete)]
        public ActionResult Delete(Guid id)
        {
            var sale = _saleRepository.Get(id);
            if (sale == null)
                return RedirectToAction("Index");

            _saleRepository.Delete(sale);
            _saleRepository.SaveChange();
            return RedirectToAction("Index");
        }
        // VIEW 
        public ActionResult View(Guid id)
        {
            var sale = _saleRepository.Get(id);
            if (sale == null)
                return RedirectToAction("Index");
            return Json(sale, JsonRequestBehavior.AllowGet);
        }
    }
}