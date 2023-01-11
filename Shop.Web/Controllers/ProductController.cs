using Shop.EntityFramework.Entities;
using Shop.EntityFramework.Infrastructures.Enums;
using Shop.EntityFramework.Infrastructures.Repository;
using Shop.Web.Common;
using Shop.Web.Common.PageFilter;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Sale> _saleRepository;

        public ProductController(IRepository<Product> productRepository, IRepository<Sale> saleRepository)
        {
            _productRepository = productRepository;
            _saleRepository = saleRepository;
        }
        // GET: Album
        public ActionResult Index(CommonPageFilter filter)
        {
            var query = _productRepository.GetQueryable().Include(x => x.Album.Songs.Select(s => s.Song))
                .WhereIf(!string.IsNullOrEmpty(filter.SearchKey), x => x.Name.ToLower().Contains(filter.SearchKey.ToLower()));
            if (string.IsNullOrEmpty(filter.Order))
                query = query.OrderBy(x => x.Code);
            else if (filter.Order == "date-desc")
                query = query.OrderByDescending(x => x.CreationTime);
            else if(filter.Order == "date-asc")
                query = query.OrderBy(x => x.CreationTime);
            else if (filter.Order == "price-desc")
                query = query.OrderByDescending(x => x.Price);
            else
                query = query.OrderBy(x => x.Price);

            var paged = query.PagedBy(filter,9).ToList();
            foreach (var item in paged)
            {
                var sale = _saleRepository.GetQueryable().Where(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now && x.Products.Contains(item.Id.ToString())).FirstOrDefault();
                if(sale != null)
                {
                    if (sale.Price.HasValue)
                        item.Price = sale.Price.Value;
                    if (sale.Percent.HasValue)
                        item.Price = item.Price - (item.Price * sale.Percent.Value) / 100;
                }
            }
            var model = new CommonPageResult<Product>()
            {
                Data = paged,
                Filter = filter,
                TotalPage = (int)Math.Ceiling((decimal)query.Count() /9)
            };

            return View(model);
        }
        public ActionResult View(string code)
        {
            var prod = _productRepository.GetQueryable().Include(x => x.Album.Songs.Select(s => s.Song)).FirstOrDefault(x=>x.Code==code);
            if (prod==null)
                return RedirectToAction("Index");
            var sale = _saleRepository.GetQueryable().Where(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now && x.Products.Contains(prod.Id.ToString())).FirstOrDefault();
            if (sale != null)
            {
                if (sale.Price.HasValue)
                    prod.Price = sale.Price.Value;
                if (sale.Percent.HasValue)
                    prod.Price = prod.Price - (prod.Price * sale.Percent.Value) / 100;
            }
            return View(prod);
        }
    }
}