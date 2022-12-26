using Shop.EntityFramework.Entities;
using Shop.EntityFramework.Infrastructures.Repository;
using Shop.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Web.Areas.Admin.Controllers
{
    public class AlbumController : Controller
    {
        private readonly IRepository<Album> _album;
        public AlbumController(IRepository<Album> album)
        {
            _album= album;
        }
        // Filter
        public ActionResult Index(CommonFilter filter)
        {
            var query = _album.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(filter.SearchKey), x => x.Code.ToLower().Contains(filter.SearchKey.ToLower()) || x.Name.ToLower().Contains(filter.SearchKey));
            var model = new CommonListResult<Album>();
            model.Filter = filter;
            model.TotalCount = query.Count();
            model.TotalPage = Math.Ceiling((decimal)query.Count() / 10);
            model.List = query.OrderByDescending(x => x.CreationTime).PagedBy(filter).ToList();
            return View(model);
        }
        // GET: Admin/Album
        public ActionResult Index()
        {
            return View();
        }
        // GET: Admin/Album/add
        public ActionResult Add()
        {
            return View();
        }
        // GET: Admin/Album/edit
        public ActionResult Edit()
        {
            return View();
        }
    }
}