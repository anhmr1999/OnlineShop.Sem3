using Shop.EntityFramework.Entities;
using Shop.EntityFramework.Infrastructures.Enums;
using Shop.EntityFramework.Infrastructures.Permissions;
using Shop.EntityFramework.Infrastructures.Repository;
using Shop.Web.Authentications;
using Shop.Web.Common;
using Shop.Web.Common.ObjectRequests;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Net.WebRequestMethods;

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
        [ShopAuthorize(Proxy = PermissionName.SongTrainerGame)]
        public ActionResult Index(SongTrailerGameAdminFilter filter)
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

        [ShopAuthorize(Proxy = PermissionName.SongTrainerGameAdd)]
        public ActionResult Add()
        {
            ViewBag.Producers = _producerRepository.GetQueryable().ToList();
            ViewBag.Categories = _categoryRepository.GetList(x => x.CateFor == true);
            ViewBag.Singers = _actorRepository.GetQueryable().ToList();
            return View(new SongTrailerGameObject());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ShopAuthorize(Proxy = PermissionName.SongTrainerGameAdd)]
        public ActionResult Add(SongTrailerGameObject songDto)
        {
            ViewBag.Producers = _producerRepository.GetQueryable().ToList();
            ViewBag.Categories = _categoryRepository.GetList(x => x.CateFor == true);
            ViewBag.Singers = _actorRepository.GetQueryable().ToList();
            var file = Request.Files.Get("img");
            if (!ModelState.IsValid)
                return View(songDto);

            if (file == null || file.ContentLength == 0)
            {
                ModelState.AddModelError(nameof(SongTrailerGameObject.Image), "Image is not empty");
                return View(songDto);
            }

            if (songDto.ActorOrSingers.Length == 0)
            {
                ModelState.AddModelError(nameof(SongTrailerGameObject.ActorOrSingers), "Actor or Singer is not empty");
                return View(songDto);
            }

            if (_songRepository.Any(x => x.Code == songDto.Code.ToLower()))
            {
                ModelState.AddModelError(nameof(SongTrailerGameObject.Code), "Code already used");
                return View(songDto);
            }

            var fileName = songDto.Code + "_" + file.FileName;
            file.SaveAs(Path.Combine(Server.MapPath("~/assets/img"), fileName));

            var song = new SongOrTrailerOrGame();
            song.Link = songDto.Link;
            song.ManufactureDate = songDto.ManufactureDate;
            song.PremiereDate = songDto.PremiereDate;
            song.Code = songDto.Code.ToLower();
            song.Name = songDto.Name;
            song.Type = SongTrailerGameTypeEnum.Song;
            song.Description = songDto.Description;
            song.AllowDownloadFree = songDto.AllowDownloadFree;
            song.ProducerId = songDto.ProducerId;
            song.CategoryId = songDto.CategoryId;
            song.Image = "/assets/img/" + fileName;
            song.ActorOrSingers = songDto.ActorOrSingers.Select(x => new SongAndSinger() { SingerId = x, SongId = song.Id }).ToList();

            song = _songRepository.Insert(song);
            _songRepository.SaveChange();
            return RedirectToAction("Index");
        }

        [ShopAuthorize(Proxy = PermissionName.SongTrainerGameEdit)]
        public ActionResult Edit(Guid id)
        {
            var song = _songRepository.GetQueryable()
                .Include(x => x.ActorOrSingers).FirstOrDefault(x => x.Id == id);
            if (song == null)
                return RedirectToAction("Index");

            ViewBag.Producers = _producerRepository.GetQueryable().ToList();
            ViewBag.Categories = _categoryRepository.GetList(x => x.CateFor == true);
            ViewBag.Singers = _actorRepository.GetQueryable().ToList();
            SongTrailerGameObject model = new SongTrailerGameObject()
            {
                ActorOrSingers = song.ActorOrSingers.Select(x => x.SingerId).ToArray(),
                Code= song.Code,
                Name= song.Name,
                ManufactureDate= song.ManufactureDate,
                PremiereDate = song.PremiereDate,
                Description= song.Description,
                AllowDownloadFree= song.AllowDownloadFree,
                ProducerId= song.ProducerId,
                CategoryId = song.CategoryId,
                Image = song.Image
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ShopAuthorize(Proxy = PermissionName.SongTrainerGameEdit)]
        public ActionResult Edit(Guid id, SongTrailerGameObject songDto)
        {
            ViewBag.Producers = _producerRepository.GetQueryable().ToList();
            ViewBag.Categories = _categoryRepository.GetList(x => x.CateFor == true);
            ViewBag.Singers = _actorRepository.GetQueryable().ToList();

            var file = Request.Files.Get("img");
            if (!ModelState.IsValid)
                return View(songDto);

            var song = _songRepository.GetQueryable()
                .Include(x => x.ActorOrSingers).FirstOrDefault(x => x.Id == id);
            if (song == null)
                return RedirectToAction("Index");

            if (songDto.ActorOrSingers.Length == 0)
            {
                ModelState.AddModelError(nameof(SongTrailerGameObject.ActorOrSingers), "Actor or Singer is not empty");
                return View(songDto);
            }

            if (_songRepository.Any(x => x.Code == songDto.Code.ToLower() && x.Id != id))
            {
                ModelState.AddModelError(nameof(SongTrailerGameObject.Code), "Code already used");
                return View(songDto);
            }

            if (file != null && file.ContentLength > 0)
            {
                if (System.IO.File.Exists(song.Image))
                    System.IO.File.Delete(song.Image);

                var fileName = songDto.Code + "_" + file.FileName;
                file.SaveAs(Path.Combine(Server.MapPath("~/assets/img"), fileName));
                song.Image = "/assets/img/" + fileName;
            }

            song.Link = songDto.Link;
            song.ManufactureDate = songDto.ManufactureDate;
            song.PremiereDate = songDto.PremiereDate;
            song.Code = songDto.Code.ToLower();
            song.Name = songDto.Name;
            song.Description = songDto.Description;
            song.AllowDownloadFree = songDto.AllowDownloadFree;
            song.ProducerId = songDto.ProducerId;
            song.CategoryId = songDto.CategoryId;

            if(song.ActorOrSingers != null)
            {
                var removeSingers = new List<SongAndSinger>();

                //remove
                foreach (var item in song.ActorOrSingers)
                    if (!songDto.ActorOrSingers.Contains(item.SingerId))
                        removeSingers.Add(item);
                foreach (var item in removeSingers)
                    song.ActorOrSingers.Remove(item);

                foreach (var item in songDto.ActorOrSingers)
                {
                    if (!song.ActorOrSingers.Any(x => x.SingerId == item))
                        song.ActorOrSingers.Add(new SongAndSinger() { SingerId = item, SongId = song.Id });
                }
            } else
                song.ActorOrSingers = songDto.ActorOrSingers.Select(x => new SongAndSinger() { SingerId = x, SongId = song.Id }).ToList();

            _songRepository.Update(song);
            _songRepository.SaveChange();
            return RedirectToAction("Index");

        }

        [ShopAuthorize(Proxy = PermissionName.SongTrainerGameDelete)]
        public ActionResult Delete(Guid id)
        {
            var song = _songRepository.Get(id);
            if (song == null)
                return RedirectToAction("Index");

            _songRepository.Delete(song);
            _songRepository.SaveChange();
            return RedirectToAction("Index");
        }

    }
}