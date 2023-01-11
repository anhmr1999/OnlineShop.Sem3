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
    public class HomeController : Controller

    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Sale> _saleRepository;
        private readonly IRepository<SongOrTrailerOrGame> _songRepository;

        public HomeController(IRepository<Product> productRepository, IRepository<Sale> saleRepository ,IRepository<SongOrTrailerOrGame> songRepository)
        {
            _productRepository = productRepository;
            _saleRepository = saleRepository;
            _songRepository = songRepository;
        }
        public ActionResult Index()
        {
            var query = _productRepository.GetQueryable().Include(x => x.Album.Songs.Select(s => s.Song));
            var paged = query.Take(4).ToList();
            foreach (var item in paged)
            {
                var sale = _saleRepository.GetQueryable().Where(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now && x.Products.Contains(item.Id.ToString())).FirstOrDefault();
                if (sale != null)
                {
                    if (sale.Price.HasValue)
                        item.Price = sale.Price.Value;
                    if (sale.Percent.HasValue)
                        item.Price = item.Price - (item.Price * sale.Percent.Value) / 100;
                }
            }
            
            var querySong = _songRepository.GetQueryable().Include(x => x.Category).Take(8).ToList();
            ViewBag.Song = querySong;
            ViewBag.Product = paged;
            return View();
        }
    }
}