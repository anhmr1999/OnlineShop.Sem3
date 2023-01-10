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
    public class GameController : Controller
    {
        private readonly IRepository<SongOrTrailerOrGame> _gameRepository;
        private readonly IRepository<Producer> _producerRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<ActorOrSinger> _actorRepository;

        public GameController(
            IRepository<SongOrTrailerOrGame> gameRepository,
            IRepository<Producer> producerRepository,
            IRepository<Category> categoryRepository,
            IRepository<ActorOrSinger> actorRepository)
        {
            _gameRepository = gameRepository;
            _producerRepository = producerRepository;
            _categoryRepository = categoryRepository;
            _actorRepository = actorRepository;
        }

        // GET: Admin/Song
        [ShopAuthorize(Proxy = PermissionName.SongTrainerGame)]
        public ActionResult Index(SongTrailerGameAdminFilter filter)
        {
            var query = _gameRepository.GetQueryable()
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
            ViewBag.Categories = _categoryRepository.GetList(x => x.CateFor == false);
            return View(model);
        }

        [ShopAuthorize(Proxy = PermissionName.SongTrainerGameAdd)]
        public ActionResult Add()
        {
            ViewBag.Producers = _producerRepository.GetQueryable().ToList();
            ViewBag.Categories = _categoryRepository.GetList(x => x.CateFor == false);
            ViewBag.Singers = _actorRepository.GetQueryable().ToList();
            return View(new SongTrailerGameObject());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ShopAuthorize(Proxy = PermissionName.SongTrainerGameAdd)]
        public ActionResult Add(SongTrailerGameObject gameDto)
        {
            ViewBag.Producers = _producerRepository.GetQueryable().ToList();
            ViewBag.Categories = _categoryRepository.GetList(x => x.CateFor == false);
            ViewBag.Singers = _actorRepository.GetQueryable().ToList();
            var file = Request.Files.Get("img");
            if (!ModelState.IsValid)
                return View(gameDto);

            if (file == null || file.ContentLength == 0)
            {
                ModelState.AddModelError(nameof(SongTrailerGameObject.Image), "Image is not empty");
                return View(gameDto);
            }

            if (gameDto.ActorOrSingers.Length == 0)
            {
                ModelState.AddModelError(nameof(SongTrailerGameObject.ActorOrSingers), "Actor or Singer is not empty");
                return View(gameDto);
            }

            if (_gameRepository.Any(x => x.Code == gameDto.Code.ToLower()))
            {
                ModelState.AddModelError(nameof(SongTrailerGameObject.Code), "Code already used");
                return View(gameDto);
            }

            var fileName = gameDto.Code + "_" + file.FileName;
            file.SaveAs(Path.Combine(Server.MapPath("~/assets/img"), fileName));

            var game = new SongOrTrailerOrGame();
            game.Link = gameDto.Link;
            game.ManufactureDate = gameDto.ManufactureDate;
            game.PremiereDate = gameDto.PremiereDate;
            game.Code = gameDto.Code;
            game.Name = gameDto.Name;
            game.Type = SongTrailerGameTypeEnum.TrailerFilm;
            game.Description = gameDto.Description;
            game.AllowDownloadFree = gameDto.AllowDownloadFree;
            game.ProducerId = gameDto.ProducerId;
            game.CategoryId = gameDto.CategoryId;
            game.Image = "/assets/img/" + fileName;
            game.ActorOrSingers = gameDto.ActorOrSingers.Select(x => new SongAndSinger() { SingerId = x, SongId = game.Id }).ToList();

            game = _gameRepository.Insert(game);
            _gameRepository.SaveChange();
            return RedirectToAction("Index");
        }

        [ShopAuthorize(Proxy = PermissionName.SongTrainerGameEdit)]
        public ActionResult Edit(Guid id)
        {
            var game = _gameRepository.GetQueryable()
                .Include(x => x.ActorOrSingers).FirstOrDefault(x => x.Id == id);
            if (game == null)
                return RedirectToAction("Index");

            ViewBag.Producers = _producerRepository.GetQueryable().ToList();
            ViewBag.Categories = _categoryRepository.GetList(x => x.CateFor == false);
            ViewBag.Singers = _actorRepository.GetQueryable().ToList();
            SongTrailerGameObject model = new SongTrailerGameObject()
            {
                ActorOrSingers = game.ActorOrSingers.Select(x => x.SingerId).ToArray(),
                Code = game.Code,
                Name = game.Name,
                ManufactureDate = game.ManufactureDate,
                PremiereDate = game.PremiereDate,
                Description = game.Description,
                AllowDownloadFree = game.AllowDownloadFree,
                ProducerId = game.ProducerId,
                CategoryId = game.CategoryId,
                Image = game.Image
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ShopAuthorize(Proxy = PermissionName.SongTrainerGameEdit)]
        public ActionResult Edit(Guid id, SongTrailerGameObject gameDto)
        {
            ViewBag.Producers = _producerRepository.GetQueryable().ToList();
            ViewBag.Categories = _categoryRepository.GetList(x => x.CateFor == false);
            ViewBag.Singers = _actorRepository.GetQueryable().ToList();

            var file = Request.Files.Get("img");
            if (!ModelState.IsValid)
                return View(gameDto);

            var game = _gameRepository.GetQueryable()
                .Include(x => x.ActorOrSingers).FirstOrDefault(x => x.Id == id);
            if (game == null)
                return RedirectToAction("Index");

            if (gameDto.ActorOrSingers.Length == 0)
            {
                ModelState.AddModelError(nameof(SongTrailerGameObject.ActorOrSingers), "Actor or Singer is not empty");
                return View(gameDto);
            }

            if (_gameRepository.Any(x => x.Code == gameDto.Code.ToLower() && x.Id != id))
            {
                ModelState.AddModelError(nameof(SongTrailerGameObject.Code), "Code already used");
                return View(gameDto);
            }

            if (file != null && file.ContentLength > 0)
            {
                if (System.IO.File.Exists(game.Image))
                    System.IO.File.Delete(game.Image);

                var fileName = gameDto.Code + "_" + file.FileName;
                file.SaveAs(Path.Combine(Server.MapPath("~/assets/img"), fileName));
                game.Image = "/assets/img/" + fileName;
            }

            game.Link = gameDto.Link;
            game.ManufactureDate = gameDto.ManufactureDate;
            game.PremiereDate = gameDto.PremiereDate;
            game.Code = gameDto.Code;
            game.Name = gameDto.Name;
            game.Description = gameDto.Description;
            game.AllowDownloadFree = gameDto.AllowDownloadFree;
            game.ProducerId = gameDto.ProducerId;
            game.CategoryId = gameDto.CategoryId;

            if (game.ActorOrSingers != null)
            {
                var removeSingers = new List<SongAndSinger>();

                //remove
                foreach (var item in game.ActorOrSingers)
                    if (!gameDto.ActorOrSingers.Contains(item.SingerId))
                        removeSingers.Add(item);
                foreach (var item in removeSingers)
                    game.ActorOrSingers.Remove(item);

                foreach (var item in gameDto.ActorOrSingers)
                {
                    if (!game.ActorOrSingers.Any(x => x.SingerId == item))
                        game.ActorOrSingers.Add(new SongAndSinger() { SingerId = item, SongId = game.Id });
                }
            }
            else
                game.ActorOrSingers = gameDto.ActorOrSingers.Select(x => new SongAndSinger() { SingerId = x, SongId = game.Id }).ToList();

            _gameRepository.Update(game);
            _gameRepository.SaveChange();
            return RedirectToAction("Index");
        }

        [ShopAuthorize(Proxy = PermissionName.SongTrainerGameDelete)]
        public ActionResult Delete(Guid id)
        {
            var game = _gameRepository.Get(id);
            if (game == null)
                return RedirectToAction("Index");

            _gameRepository.Delete(game);
            _gameRepository.SaveChange();
            return RedirectToAction("Index");
        }
    }
}