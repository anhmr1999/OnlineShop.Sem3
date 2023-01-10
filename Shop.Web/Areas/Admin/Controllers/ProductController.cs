using Shop.EntityFramework.Entities;
using Shop.EntityFramework.Infrastructures.Permissions;
using Shop.EntityFramework.Infrastructures.Repository;
using Shop.Web.Authentications;
using Shop.Web.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Web.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Supplier> _supplierRepository;
        private readonly IRepository<Album> _albumRepository;

        public ProductController(IRepository<Product> productRepository, IRepository<Supplier> supplierRepository, IRepository<Album> albumRepository)
        {
            _productRepository = productRepository;
            _supplierRepository = supplierRepository;
            _albumRepository = albumRepository;
        }

        // GET: Admin/Product
        [ShopAuthorize(Proxy = PermissionName.Product)]
        public ActionResult Index(ProductAdminFilter filter)
        {
            var query = _productRepository.GetQueryable()
                .Include(x => x.Album).Include(x => x.Supplier)
                .WhereIf(!string.IsNullOrEmpty(filter.SearchKey), x => x.Album.Name.ToLower().Contains(filter.SearchKey.ToLower()))
                .WhereIf(filter.SupplierId.HasValue, x => x.SupplierId == filter.SupplierId);

            var model = new CommonListResult<Product>();
            model.Filter = filter;
            model.TotalCount = query.Count();
            model.TotalPage = Math.Ceiling((decimal)model.TotalCount / 10);
            model.List = query.OrderByDescending(x => x.CreationTime).PagedBy(filter).ToList();

            ViewBag.Suppliers = _supplierRepository.GetQueryable().ToList();
            return View(model);
        }

        [ShopAuthorize(Proxy = PermissionName.ProductAdd)]
        public ActionResult Add()
        {
            ViewBag.Suppliers = _supplierRepository.GetQueryable().ToList();
            ViewBag.Albums = _albumRepository.GetQueryable().ToList();

            return View(new Product());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ShopAuthorize(Proxy = PermissionName.ProductAdd)]
        public ActionResult Add(Product product)
        {
            ViewBag.Suppliers = _supplierRepository.GetQueryable().ToList();
            ViewBag.Albums = _albumRepository.GetQueryable().ToList();
            if(!ModelState.IsValid)
                return View(product);

            if(_productRepository.Any(x => x.Code.ToLower() == product.Code))
            {
                ModelState.AddModelError(nameof(Product.Code), "code already used");
                return View(product);
            }    

            _productRepository.Insert(product);
            _productRepository.SaveChange();

            return RedirectToAction("Index");
        }

        [ShopAuthorize(Proxy = PermissionName.ProductEdit)]
        public ActionResult Edit(Guid id)
        {
            var product = _productRepository.Get(id);

            if(product == null)
                return RedirectToAction("Index");

            ViewBag.Suppliers = _supplierRepository.GetQueryable().ToList();
            ViewBag.Albums = _albumRepository.GetQueryable().ToList();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ShopAuthorize(Proxy = PermissionName.ProductEdit)]
        public ActionResult Edit(Guid id, Product productDto)
        {
            ViewBag.Suppliers = _supplierRepository.GetQueryable().ToList();
            ViewBag.Albums = _albumRepository.GetQueryable().ToList();
            if (!ModelState.IsValid)
                return View(productDto);

            var product = _productRepository.Get(id);

            if (product == null)
                return RedirectToAction("Index");

            if (_productRepository.Any(x => x.Code.ToLower() == productDto.Code && x.Id != id))
            {
                ModelState.AddModelError(nameof(Product.Code), "code already used");
                return View(productDto);
            }

            product.Code = productDto.Code;
            product.Name = productDto.Name;
            product.AlbumId = productDto.AlbumId;
            product.Price = productDto.Price;
            product.SupplierId = productDto.SupplierId;

            _productRepository.Update(product);
            _productRepository.SaveChange();

            return RedirectToAction("Index");
        }

        [ShopAuthorize(Proxy = PermissionName.ProductDelete)]
        public ActionResult Delete(Guid id)
        {
            var product = _productRepository.Get(id);
            if (product == null)
                return RedirectToAction("Index");

            _productRepository.Delete(product);
            _productRepository.SaveChange();
            return RedirectToAction("Index");
        }
    }
}