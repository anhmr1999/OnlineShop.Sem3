using Shop.EntityFramework.Entities;
using Shop.EntityFramework.Infrastructures.Enums;
using Shop.EntityFramework.Infrastructures.Repository;
using Shop.Web.Common;
using Shop.Web.Common.ObjectRequests;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Web.Areas.Admin.Controllers
{
    public class SongController : Controller
    {
        private readonly IRepository<SongOrTrailerOrGame> _songRepository;
        private readonly IRepository<Producer> _producerRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<ActorOrSinger> _actorRepository;

        public SongController(
            IRepository<SongOrTrailerOrGame> songRepository, 
            IRepository<Producer> producerRepository, 
            IRepository<Category> categoryRepository, 
            IRepository<ActorOrSinger> actorRepository)
        {
            _songRepository = songRepository;
            _producerRepository = producerRepository;
            _categoryRepository = categoryRepository;
            _actorRepository = actorRepository;
        }

        // GET: Admin/Song
        public ActionResult Index(SongTrailerGameFilter filter)
        {
            var query = _songRepository.GetQueryable()
                .Include(x => x.Category).Include(x => x.Producer)
                .Where(x => x.Type == SongTrailerGameTypeEnum.Song)
                .WhereIf(!string.IsNullOrEmpty(filter.SearchKey), x => x.Code.ToLower().Contains(filter.SearchKey.ToLower()) || x.Name.ToLower().Contains(filter.SearchKey.ToLower()))
                .WhereIf(filter.AllowDownloadFree.HasValue, x => x.AllowDownloadFree == filter.AllowDownloadFree)
                .WhereIf(filter.ProducerId.HasValue, x => x.ProducerId == filter.ProducerId)
                .WhereIf(filter.CategoryId.HasValue, x => x.CategoryId == filter.CategoryId);
            var total = query.Count();
            var model = new CommonListResult<SongOrTrailerOrGame>()
            {
                Filter = filter,
                List = query.OrderByDescending(x => x.CreationTime).Skip((filter.PageIndex - 1) * 10).Take(10).ToList(),
                TotalCount = total,
                TotalPage = (int)Math.Ceiling((decimal)total / 10)
            };
            ViewBag.Producers = _producerRepository.GetQueryable().ToList();
            ViewBag.Categories = _categoryRepository.GetList(x => x.CateFor == true);
            return View(model);
        }

        public ActionResult Add()
        {
            ViewBag.Producers = _producerRepository.GetQueryable().ToList();
            ViewBag.Categories = _categoryRepository.GetList(x => x.CateFor == true);
            return View(new SongTralerGameObject());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(SongTralerGameObject songDto)
        {
            ViewBag.Producers = _producerRepository.GetQueryable().ToList();
            ViewBag.Categories = _categoryRepository.GetList(x => x.CateFor == true);
            if (!ModelState.IsValid)
                return View(songDto);

            if (string.IsNullOrEmpty(songDto.ActorOrSingers))
            {
                ModelState.AddModelError(nameof(SongTralerGameObject.ActorOrSingers), "Actor or Singer is not empty");
                return View(songDto);
            }

            var song = new SongOrTrailerOrGame();
            song.ManufactureDate = songDto.ManufactureDate;
            song.PremiereDate = songDto.PremiereDate;
            song.Code = songDto.Code;
            song.Name = songDto.Name;
            song.Description = songDto.Description;
            song.AllowDownloadFree = songDto.AllowDownloadFree;
            song.ProducerId = songDto.ProducerId;
            song.CategoryId = songDto.CategoryId;
            song.ActorOrSingers = new List<ActorOrSinger>();
            foreach (var item in songDto.ActorOrSingers.Replace(" ", string.Empty).Split(','))
            {
                var actor = _actorRepository.Get(Guid.Parse(item));
                if (actor != null)
                {
                    song.ActorOrSingers.Add(actor);
                }
            }

            if (song.ActorOrSingers.Count == 0)
            {
                ModelState.AddModelError(nameof(SongTralerGameObject.ActorOrSingers), "Actor or Singer is not empty");
                return View(songDto);
            }

            _songRepository.Insert(song);
            _songRepository.SaveChange();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(Guid id)
        {
            ViewBag.Producers = _producerRepository.GetQueryable().ToList();
            ViewBag.Categories = _categoryRepository.GetList(x => x.CateFor == true);
            return View(new SongTralerGameObject());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, SongTralerGameObject songDto)
        {
            ViewBag.Producers = _producerRepository.GetQueryable().ToList();
            ViewBag.Categories = _categoryRepository.GetList(x => x.CateFor == true);
            if (!ModelState.IsValid)
                return View(songDto);

            if (string.IsNullOrEmpty(songDto.ActorOrSingers))
            {
                ModelState.AddModelError(nameof(SongTralerGameObject.ActorOrSingers), "Actor or Singer is not empty");
                return View(songDto);
            }

            var song = _songRepository.Get(id);
            if(song == null)
                return RedirectToAction("Index");

            song.ManufactureDate = songDto.ManufactureDate;
            song.PremiereDate = songDto.PremiereDate;
            song.Code = songDto.Code;
            song.Name = songDto.Name;
            song.Description = songDto.Description;
            song.AllowDownloadFree = songDto.AllowDownloadFree;
            song.ProducerId = songDto.ProducerId;
            song.CategoryId = songDto.CategoryId;
            song.ActorOrSingers = new List<ActorOrSinger>();
            foreach (var item in songDto.ActorOrSingers.Replace(" ", string.Empty).Split(','))
            {
                var actor = _actorRepository.Get(Guid.Parse(item));
                if (actor != null)
                {
                    song.ActorOrSingers.Add(actor);
                }
            }

            if (song.ActorOrSingers.Count == 0)
            {
                ModelState.AddModelError(nameof(SongTralerGameObject.ActorOrSingers), "Actor or Singer is not empty");
                return View(songDto);
            }

            _songRepository.Update(song);
            _songRepository.SaveChange();
            return RedirectToAction("Index");
        }
    }
}