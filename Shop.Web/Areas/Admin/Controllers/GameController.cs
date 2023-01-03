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

        public ActionResult Add()
        {
            ViewBag.Producers = _producerRepository.GetQueryable().ToList();
            ViewBag.Categories = _categoryRepository.GetList(x => x.CateFor == false);
            ViewBag.Singers = _actorRepository.GetQueryable().ToList();
            return View(new SongTrailerGameObject());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(SongTrailerGameObject trailerDto)
        {
            ViewBag.Producers = _producerRepository.GetQueryable().ToList();
            ViewBag.Categories = _categoryRepository.GetList(x => x.CateFor == false);
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

            if (_gameRepository.Any(x => x.Code == trailerDto.Code.ToLower()))
            {
                ModelState.AddModelError(nameof(SongTrailerGameObject.Code), "Code already used");
                return View(trailerDto);
            }

            var fileName = trailerDto.Code + "_" + file.FileName;
            file.SaveAs(Path.Combine(Server.MapPath("~/assets/img"), fileName));

            var game = new SongOrTrailerOrGame();
            game.ManufactureDate = trailerDto.ManufactureDate;
            game.PremiereDate = trailerDto.PremiereDate;
            game.Code = trailerDto.Code;
            game.Name = trailerDto.Name;
            game.Type = SongTrailerGameTypeEnum.TrailerFilm;
            game.Description = trailerDto.Description;
            game.AllowDownloadFree = trailerDto.AllowDownloadFree;
            game.ProducerId = trailerDto.ProducerId;
            game.CategoryId = trailerDto.CategoryId;
            game.Image = "/assets/img/" + fileName;
            game.ActorOrSingers = trailerDto.ActorOrSingers.Select(x => new SongAndSinger() { SingerId = x, SongId = game.Id }).ToList();

            game = _gameRepository.Insert(game);
            _gameRepository.SaveChange();
            return RedirectToAction("Index");
        }

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
        public ActionResult Edit(Guid id, SongTrailerGameObject trailerDto)
        {
            ViewBag.Producers = _producerRepository.GetQueryable().ToList();
            ViewBag.Categories = _categoryRepository.GetList(x => x.CateFor == false);
            ViewBag.Singers = _actorRepository.GetQueryable().ToList();

            var file = Request.Files.Get("img");
            if (!ModelState.IsValid)
                return View(trailerDto);

            var game = _gameRepository.GetQueryable()
                .Include(x => x.ActorOrSingers).FirstOrDefault(x => x.Id == id);
            if (game == null)
                return RedirectToAction("Index");

            if (trailerDto.ActorOrSingers.Length == 0)
            {
                ModelState.AddModelError(nameof(SongTrailerGameObject.ActorOrSingers), "Actor or Singer is not empty");
                return View(trailerDto);
            }

            if (_gameRepository.Any(x => x.Code == trailerDto.Code.ToLower() && x.Id != id))
            {
                ModelState.AddModelError(nameof(SongTrailerGameObject.Code), "Code already used");
                return View(trailerDto);
            }

            if (file != null && file.ContentLength > 0)
            {
                if (System.IO.File.Exists(game.Image))
                    System.IO.File.Delete(game.Image);

                var fileName = trailerDto.Code + "_" + file.FileName;
                file.SaveAs(Path.Combine(Server.MapPath("~/assets/img"), fileName));
                game.Image = "/assets/img/" + fileName;
            }

            game.ManufactureDate = trailerDto.ManufactureDate;
            game.PremiereDate = trailerDto.PremiereDate;
            game.Code = trailerDto.Code;
            game.Name = trailerDto.Name;
            game.Description = trailerDto.Description;
            game.AllowDownloadFree = trailerDto.AllowDownloadFree;
            game.ProducerId = trailerDto.ProducerId;
            game.CategoryId = trailerDto.CategoryId;

            if (game.ActorOrSingers != null)
            {
                var removeSingers = new List<SongAndSinger>();

                //remove
                foreach (var item in game.ActorOrSingers)
                    if (!trailerDto.ActorOrSingers.Contains(item.SingerId))
                        removeSingers.Add(item);
                foreach (var item in removeSingers)
                    game.ActorOrSingers.Remove(item);

                foreach (var item in trailerDto.ActorOrSingers)
                {
                    if (!game.ActorOrSingers.Any(x => x.SingerId == item))
                        game.ActorOrSingers.Add(new SongAndSinger() { SingerId = item, SongId = game.Id });
                }
            }
            else
                game.ActorOrSingers = trailerDto.ActorOrSingers.Select(x => new SongAndSinger() { SingerId = x, SongId = game.Id }).ToList();

            _gameRepository.Update(game);
            _gameRepository.SaveChange();
            return RedirectToAction("Index");
        }
    }
}