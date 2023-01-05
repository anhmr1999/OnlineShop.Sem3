using Shop.EntityFramework.Entities;
using Shop.EntityFramework.Infrastructures.Permissions;
using Shop.EntityFramework.Infrastructures.Repository;
using Shop.Web.Authentications;
using Shop.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Web.Areas.Admin.Controllers
{
    public class SupplierController : Controller
    {
        private readonly IRepository<Supplier> _supplierRepository;

        public SupplierController(IRepository<Supplier> supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        // GET: Admin/Supplier
        [ShopAuthorize(Proxy = PermissionName.Supplier)]
        public ActionResult Index(CommonFilter filter)
        {
            var query = _supplierRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(filter.SearchKey), x => x.Code.ToLower().Contains(filter.SearchKey.ToLower()) || x.Name.ToLower().Contains(filter.SearchKey.ToLower()));

            var model = new CommonListResult<Supplier>()
            {
                Filter= filter,
                List = query.OrderByDescending(x => x.CreationTime).PagedBy(filter).ToList(),
                TotalCount= query.Count(),
                TotalPage = (int) Math.Ceiling((decimal)query.Count()/10)
            };
            return View(model);
        }

        // GET: Admin/Add
        [ShopAuthorize(Proxy = PermissionName.SupplierAdd)]
        public ActionResult Add()
        {
            return View(new Supplier());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ShopAuthorize(Proxy = PermissionName.SupplierAdd)]
        public ActionResult Add(Supplier supplier)
        {
            if(!ModelState.IsValid)
                return View(supplier);

            _supplierRepository.Insert(supplier);
            _supplierRepository.SaveChange();
            return RedirectToAction("Index");
        }

        [ShopAuthorize(Proxy = PermissionName.SupplierEdit)]
        public ActionResult Edit(Guid id)
        {
            var supplier = _supplierRepository.Get(id);
            if (supplier == null)
                return RedirectToAction("Index");

            return View(supplier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ShopAuthorize(Proxy = PermissionName.SupplierEdit)]
        public ActionResult Edit(Guid id, Supplier supplierDto)
        {
            if (!ModelState.IsValid)
                return View(supplierDto);

            var supplier = _supplierRepository.Get(id);
            if (supplier == null)
                return RedirectToAction("Index");

            if(_supplierRepository.Any(x => x.Id != id && x.Code == supplierDto.Code))
                return View(supplierDto);

            supplier.Code = supplierDto.Code;
            supplier.Name = supplierDto.Name;
            supplier.Description = supplierDto.Description;
            _supplierRepository.Update(supplier);
            _supplierRepository.SaveChange();
            return RedirectToAction("Index");
        }
    }
}