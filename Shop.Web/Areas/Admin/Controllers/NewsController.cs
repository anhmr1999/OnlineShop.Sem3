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
    public class NewsController : Controller
    {
        private readonly IRepository<News> _newsRepository;

        public NewsController(IRepository<News> newsRepository)
        {
            _newsRepository = newsRepository;
        }
        // GET: Admin/News
        [ShopAuthorize(Proxy = PermissionName.News)]
        public ActionResult Index(CommonFilter filter)
        {
            var query = _newsRepository.GetQueryable()
              .WhereIf(!string.IsNullOrEmpty(filter.SearchKey), x => x.Name.ToLower().Contains(filter.SearchKey) || x.Description.ToLower().Contains(filter.SearchKey));
            var model = new CommonListResult<News>();
            model.Filter = filter;
            model.TotalCount = query.Count();
            model.TotalPage = Math.Ceiling((decimal)query.Count() / 10);
            model.List = query.OrderByDescending(x => x.CreationTime).PagedBy(filter).ToList();
            return View(model);
        }

        // GET: Admin/Category/Add
        [ShopAuthorize(Proxy = PermissionName.NewsAdd)]
        public ActionResult Add()
        {
            return View(new News());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ShopAuthorize(Proxy = PermissionName.NewsAdd)]
        public ActionResult Add(News newsDto)
        {
            if (!ModelState.IsValid)
                return View(newsDto);
            if (_newsRepository.Any(x => x.Code.ToLower() == newsDto.Code.ToLower()))
            {
                ModelState.AddModelError(nameof(News.Code), $"code {newsDto.Code} has been used");
                return View(newsDto);
            }
            var news =new News();
            news.Code = newsDto.Code.ToLower();
            news.Name = newsDto.Name;
            news.Description = newsDto.Description;
            news.ShortDesciption = newsDto.ShortDesciption;
            news.Image = newsDto.Image;
            news.IsGlobal = newsDto.IsGlobal;
            _newsRepository.Insert(news);
            _newsRepository.SaveChange();
            return RedirectToAction("Index");
        }
        // POST: Admin/Edit
        [ShopAuthorize(Proxy = PermissionName.NewsEdit)]
        public ActionResult Edit(Guid id)
        {
            var sale = _newsRepository.Get(id);
            if (sale == null)
                return RedirectToAction("Index");
            return View(sale);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ShopAuthorize(Proxy = PermissionName.NewsEdit)]
        public ActionResult Edit(Guid id, News newsDto)
        {
            if (!ModelState.IsValid)
                return View(newsDto);
            var news = _newsRepository.Get(id);
            news.Code = newsDto.Code.ToLower();
            news.Name = newsDto.Name;
            news.Description = newsDto.Description;
            news.ShortDesciption = newsDto.ShortDesciption;
            news.Image = newsDto.Image;
            news.IsGlobal = newsDto.IsGlobal;
            _newsRepository.Update(news);
            _newsRepository.SaveChange();
            return RedirectToAction("Index");
        }
        //DELETE: 
        public ActionResult Delete(Guid id)
        {
            var sale = _newsRepository.Get(id);
            if (sale == null)
                return RedirectToAction("Index");
            _newsRepository.Delete(sale);
            _newsRepository.SaveChange();
            return RedirectToAction("Index");
        }
        // VIEW 
        public ActionResult View(Guid id)
        {
            var news = _newsRepository.Get(id);
            if (news == null)
                return RedirectToAction("Index");
            return Json(news, JsonRequestBehavior.AllowGet);
        }

    }
}