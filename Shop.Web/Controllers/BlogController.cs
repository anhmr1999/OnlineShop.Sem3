using Shop.EntityFramework.Entities;
using Shop.EntityFramework.Infrastructures.Repository;
using Shop.Web.Common;
using Shop.Web.Common.PageFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Web.Controllers
{
    public class BlogController : Controller
    {
        private readonly IRepository<News> _newRepositoy;
        private readonly IRepository<SongOrTrailerOrGame> _songRepositoy;

        public BlogController(IRepository<News> newRepositoy, IRepository<SongOrTrailerOrGame> songRepositoy)
        {
            _newRepositoy = newRepositoy;
            _songRepositoy = songRepositoy;
        }

        // GET: Blog
        public ActionResult Index(CommonFilter filter)
        {
            var list = _newRepositoy.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(filter.SearchKey), x => x.Name.ToLower().Contains(filter.SearchKey.ToLower()));
            var count = list.Count();
            var paged = list.OrderBy(x => x.CreationTime)
                .PagedBy(filter, 3).ToList();
            var model = new CommonListResult<News>()
            {
                Filter = filter,
                List = paged,
                TotalCount = count,
                TotalPage = (int)Math.Ceiling((decimal)count / 3)
            };
            ViewBag.Songs = _songRepositoy.GetQueryable().OrderBy(x => x.CreationTime).Take(5).ToList();
            return View(model);
        }

        public ActionResult View(string code)
        {
            var blog = _newRepositoy.GetQueryable()
                .FirstOrDefault(x => x.Code == code);
            if (blog == null)
                return RedirectToAction("Index");

            var songs = _songRepositoy.GetQueryable().OrderBy(x => x.CreationTime).Take(5).ToList();
            var detailt = new ViewDetailt<News, SongOrTrailerOrGame>()
            {
                Data = blog,
                LikeData = songs
            };
            return View(detailt);
        }
    }
}