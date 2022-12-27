﻿using Shop.EntityFramework.Entities;
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
     
        // GET: Admin/Album/add
        public ActionResult Add()
        {
          return  View(new Album());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Album album)
        {
            if (!ModelState.IsValid)
                return View(album);

            if (_album.Any(x => x.Code.ToLower() == album.Code.ToLower()))
            {
                ModelState.AddModelError(nameof(Album.Code), $"code {album.Code} has been used");
                return View(album);
            }

            album.Code = album.Code.ToLower();
            _album.Insert(album);
            _album.SaveChange();
            return RedirectToAction("Index");

        }
        // GET: Admin/Album/edit
        public ActionResult Edit(Guid id)
        {
            var album = _album.Get(id);
            if (album == null)
                return RedirectToAction("Index");
            return View(album);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, Album album)
        {
            if (!ModelState.IsValid)
                return View(album);

            if (_album.Any(x => x.Code.ToLower() == album.Code.ToLower() && x.Id != id))
            {
                ModelState.AddModelError(nameof(Album.Code), $"code {album.Code} has been used");
                return View(album);
            }
            var cate = _album.Get(id);
            if (cate == null)
                return RedirectToAction("Index");

            cate.Code = album.Code.ToLower();
            cate.Name = album.Name;
            cate.ReleaseDate = album.ReleaseDate;
            cate.Description = album.Description;

            _album.Update(cate);
            _album.SaveChange();
            return RedirectToAction("Index");
        }
    }
}