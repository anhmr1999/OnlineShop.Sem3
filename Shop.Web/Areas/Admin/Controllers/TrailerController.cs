using Shop.EntityFramework.Entities;
using Shop.EntityFramework.Infrastructures.Enums;
using Shop.EntityFramework.Infrastructures.Repository;
using Shop.Web.Common.ObjectRequests;
using Shop.Web.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Shop.EntityFramework.Infrastructures.Permissions;
using Shop.Web.Authentications;

namespace Shop.Web.Areas.Admin.Controllers
{
    public class TrailerController : Controller
    {
        private readonly IRepository<SongOrTrailerOrGame> _trailerRepository;
        private readonly IRepository<Producer> _producerRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<ActorOrSinger> _actorRepository;

        public TrailerController(
            IRepository<SongOrTrailerOrGame> songRepository,
            IRepository<Producer> producerRepository,
            IRepository<Category> categoryRepository,
            IRepository<ActorOrSinger> actorRepository)
        {
            _trailerRepository = songRepository;
            _producerRepository = producerRepository;
            _categoryRepository = categoryRepository;
            _actorRepository = actorRepository;
        }

        // GET: Admin/Song
        [ShopAuthorize(Proxy = PermissionName.SongTrainerGame)]
        public ActionResult Index(SongTrailerGameAdminFilter filter)
        {
            var query = _trailerRepository.GetQueryable()
                .Include(x => x.Category).Include(x => x.Producer)
                .Where(x => x.Type == SongTrailerGameTypeEnum.TrailerFilm)
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
            ViewBag.Categories = _categoryRepository.GetList(x => x.CateFor == null);
            return View(model);
        }

        [ShopAuthorize(Proxy = PermissionName.SongTrainerGameAdd)]
        public ActionResult Add()
        {
            ViewBag.Producers = _producerRepository.GetQueryable().ToList();
            ViewBag.Categories = _categoryRepository.GetList(x => x.CateFor == null);
            ViewBag.Singers = _actorRepository.GetQueryable().ToList();
            return View(new SongTrailerGameObject());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ShopAuthorize(Proxy = PermissionName.SongTrainerGameAdd)]
        public ActionResult Add(SongTrailerGameObject trailerDto)
        {
            ViewBag.Producers = _producerRepository.GetQueryable().ToList();
            ViewBag.Categories = _categoryRepository.GetList(x => x.CateFor == null);
            ViewBag.Singers = _actorRepository.GetQueryable().ToList();
            var file = Request.Files.Get("img");
            if (!ModelState.IsValid)
                return View(trailerDto);

            if (file == null || file.ContentLength == 0)
            {
                ModelState.AddModelError(nameof(SongTrailerGameObject.Image), "Image is not empty");
                return View(trailerDto);
            }

            if (trailerDto.ActorOrSingers.Length == 0)
            {
                ModelState.AddModelError(nameof(SongTrailerGameObject.ActorOrSingers), "Actor or Singer is not empty");
                return View(trailerDto);
            }


            if (_trailerRepository.Any(x => x.Code == trailerDto.Code.ToLower()))
            {
                ModelState.AddModelError(nameof(SongTrailerGameObject.Code), "Code already used");
                return View(trailerDto);
            }

            var fileName = trailerDto.Code + "_" + file.FileName;
            file.SaveAs(Path.Combine(Server.MapPath("~/assets/img"), fileName));

            var trailer = new SongOrTrailerOrGame();
            trailer.Link = trailerDto.Link;
            trailer.ManufactureDate = trailerDto.ManufactureDate;
            trailer.PremiereDate = trailerDto.PremiereDate;
            trailer.Code = trailerDto.Code;
            trailer.Name = trailerDto.Name;
            trailer.Type = SongTrailerGameTypeEnum.TrailerFilm;
            trailer.Description = trailerDto.Description;
            trailer.AllowDownloadFree = trailerDto.AllowDownloadFree;
            trailer.ProducerId = trailerDto.ProducerId;
            trailer.CategoryId = trailerDto.CategoryId;
            trailer.Image = "/assets/img/" + fileName;
            trailer.ActorOrSingers = trailerDto.ActorOrSingers.Select(x => new SongAndSinger() { SingerId = x, SongId = trailer.Id }).ToList();

            trailer = _trailerRepository.Insert(trailer);
            _trailerRepository.SaveChange();
            return RedirectToAction("Index");
        }

        [ShopAuthorize(Proxy = PermissionName.SongTrainerGameEdit)]
        public ActionResult Edit(Guid id)
        {
            var trailer = _trailerRepository.GetQueryable()
                .Include(x => x.ActorOrSingers).FirstOrDefault(x => x.Id == id);
            if (trailer == null)
                return RedirectToAction("Index");

            ViewBag.Producers = _producerRepository.GetQueryable().ToList();
            ViewBag.Categories = _categoryRepository.GetList(x => x.CateFor == null);
            ViewBag.Singers = _actorRepository.GetQueryable().ToList();
            SongTrailerGameObject model = new SongTrailerGameObject()
            {
                ActorOrSingers = trailer.ActorOrSingers.Select(x => x.SingerId).ToArray(),
                Code = trailer.Code,
                Name = trailer.Name,
                ManufactureDate = trailer.ManufactureDate,
                PremiereDate = trailer.PremiereDate,
                Description = trailer.Description,
                AllowDownloadFree = trailer.AllowDownloadFree,
                ProducerId = trailer.ProducerId,
                CategoryId = trailer.CategoryId,
                Image = trailer.Image
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ShopAuthorize(Proxy = PermissionName.SongTrainerGameEdit)]
        public ActionResult Edit(Guid id, SongTrailerGameObject trailerDto)
        {
            ViewBag.Producers = _producerRepository.GetQueryable().ToList();
            ViewBag.Categories = _categoryRepository.GetList(x => x.CateFor == null);
            ViewBag.Singers = _actorRepository.GetQueryable().ToList();

            var file = Request.Files.Get("img");
            if (!ModelState.IsValid)
                return View(trailerDto);

            var trailer = _trailerRepository.GetQueryable()
                .Include(x => x.ActorOrSingers).FirstOrDefault(x => x.Id == id);
            if (trailer == null)
                return RedirectToAction("Index");

            if (trailerDto.ActorOrSingers.Length == 0)
            {
                ModelState.AddModelError(nameof(SongTrailerGameObject.ActorOrSingers), "Actor or Singer is not empty");
                return View(trailerDto);
            }

            if (_trailerRepository.Any(x => x.Code == trailerDto.Code.ToLower() && x.Id != id))
            {
                ModelState.AddModelError(nameof(SongTrailerGameObject.Code), "Code already used");
                return View(trailerDto);
            }

            if (file != null && file.ContentLength > 0)
            {
                if (System.IO.File.Exists(trailer.Image))
                    System.IO.File.Delete(trailer.Image);

                var fileName = trailerDto.Code + "_" + file.FileName;
                file.SaveAs(Path.Combine(Server.MapPath("~/assets/img"), fileName));
                trailer.Image = "/assets/img/" + fileName;
            }

            trailer.Link = trailerDto.Link;
            trailer.ManufactureDate = trailerDto.ManufactureDate;
            trailer.PremiereDate = trailerDto.PremiereDate;
            trailer.Code = trailerDto.Code;
            trailer.Name = trailerDto.Name;
            trailer.Description = trailerDto.Description;
            trailer.AllowDownloadFree = trailerDto.AllowDownloadFree;
            trailer.ProducerId = trailerDto.ProducerId;
            trailer.CategoryId = trailerDto.CategoryId;

            if (trailer.ActorOrSingers != null)
            {
                var removeSingers = new List<SongAndSinger>();

                //remove
                foreach (var item in trailer.ActorOrSingers)
                    if (!trailerDto.ActorOrSingers.Contains(item.SingerId))
                        removeSingers.Add(item);
                foreach (var item in removeSingers)
                    trailer.ActorOrSingers.Remove(item);

                foreach (var item in trailerDto.ActorOrSingers)
                {
                    if (!trailer.ActorOrSingers.Any(x => x.SingerId == item))
                        trailer.ActorOrSingers.Add(new SongAndSinger() { SingerId = item, SongId = trailer.Id });
                }
            }
            else
                trailer.ActorOrSingers = trailerDto.ActorOrSingers.Select(x => new SongAndSinger() { SingerId = x, SongId = trailer.Id }).ToList();

            _trailerRepository.Update(trailer);
            _trailerRepository.SaveChange();
            return RedirectToAction("Index");
        }

        [ShopAuthorize(Proxy = PermissionName.SongTrainerGameDelete)]
        public ActionResult Delete(Guid id)
        {
            var trailer = _trailerRepository.Get(id);
            if (trailer == null)
                return RedirectToAction("Index");

            _trailerRepository.Delete(trailer);
            _trailerRepository.SaveChange();
            return RedirectToAction("Index");
        }
    }
}