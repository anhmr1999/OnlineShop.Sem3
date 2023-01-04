using Shop.EntityFramework.Entities;
using Shop.EntityFramework.Infrastructures.Repository;
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
        public ActionResult Index()
        {
            return View(new News());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(News news)
        {
            if (!ModelState.IsValid)
                return View(news);

            if (_newsRepository.Any(x => x.Code.ToLower() == news.Code.ToLower()))
            {
                ModelState.AddModelError(nameof(News.Code), $"code {news.Code} has been used");
                return View(news);
            }

            news.Code = news.Code.ToLower();
            _newsRepository.Insert(news);
            _newsRepository.SaveChange();
            return RedirectToAction("Index");
        }
        public ActionResult Edit()
        {
            return View();
        }
    }
}