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
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Web.Areas.Admin.Controllers
{
    public class AlbumController : Controller
    {
        private readonly IRepository<Album> _albumRepository;
        private readonly IRepository<SongOrTrailerOrGame> _songOrTrailerRepository;

        public AlbumController(IRepository<Album> albumRepository, IRepository<SongOrTrailerOrGame> songOrTrailerRepository)
        {
            _albumRepository = albumRepository;
            _songOrTrailerRepository = songOrTrailerRepository;
        }

        // GET: Admin/Album
        [ShopAuthorize(Proxy = PermissionName.Album)]
        public ActionResult Index(CommonFilter filter)
        {
            var query = _albumRepository.GetQueryable().Include(x => x.Songs)
                .WhereIf(!string.IsNullOrEmpty(filter.SearchKey), x => x.Code.ToLower().Contains(filter.SearchKey.ToLower()) || x.Name.ToLower().Contains(filter.SearchKey));
            var model = new CommonListResult<Album>();
            model.Filter = filter;
            model.TotalCount = query.Count();
            model.TotalPage = Math.Ceiling((decimal)query.Count() / 10);
            model.List = query.OrderByDescending(x => x.CreationTime).PagedBy(filter).ToList();
            return View(model);
        }

        // GET: Admin/Album/add
        [ShopAuthorize(Proxy = PermissionName.AlbumAdd)]
        public ActionResult Add()
        {
            ViewBag.Songs = _songOrTrailerRepository.GetList(x => x.Type == SongTrailerGameTypeEnum.Song);
            return View(new AlbumObject());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ShopAuthorize(Proxy = PermissionName.AlbumAdd)]
        public ActionResult Add(AlbumObject albumDto)
        {
            ViewBag.Songs = _songOrTrailerRepository.GetList(x => x.Type == SongTrailerGameTypeEnum.Song);
            if (!ModelState.IsValid)
                return View(albumDto);
            if (albumDto.Songs == null || albumDto.Songs.Length == 0)
            {
                ModelState.AddModelError(nameof(AlbumObject.Songs), "Songs is not empty");
                return View(albumDto);
            }
            if(_albumRepository.Any(x => x.Code.ToLower() == albumDto.Code.ToLower()))
            {
                ModelState.AddModelError(nameof(AlbumObject.Code), "Code already used");
                return View(albumDto);
            }
            var album = new Album();
            album.Code = albumDto.Code.ToLower();
            album.Name = albumDto.Name;
            album.Description = albumDto.Description;
            album.ReleaseDate= albumDto.ReleaseDate;
            album.Songs = albumDto.Songs.Select(x => new AlbumDetails() { SongId = x }).ToList();
            _albumRepository.Insert(album);
            _albumRepository.SaveChange();
            return RedirectToAction("Index");
        }

        // GET: Admin/Album/edit
        [ShopAuthorize(Proxy = PermissionName.AlbumEdit)]
        public ActionResult Edit(Guid id)
        {
            var album = _albumRepository.GetQueryable().Include(x => x.Songs).FirstOrDefault(x => x.Id == id);
            if(album == null)
                return RedirectToAction("Index");

            var model = new AlbumObject()
            {
                Code = album.Code,
                Name = album.Name,
                Description = album.Description,
                ReleaseDate = album.ReleaseDate,
                Songs = album.Songs.Select(x => x.SongId).ToArray()
            };

            ViewBag.Songs = _songOrTrailerRepository.GetList(x => x.Type == SongTrailerGameTypeEnum.Song);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ShopAuthorize(Proxy = PermissionName.AlbumEdit)]
        public ActionResult Edit(Guid id, AlbumObject albumDto)
        {
            ViewBag.Songs = _songOrTrailerRepository.GetList(x => x.Type == SongTrailerGameTypeEnum.Song);
            if (!ModelState.IsValid)
                return View(albumDto);
            if (albumDto.Songs == null || albumDto.Songs.Length == 0)
            {
                ModelState.AddModelError(nameof(AlbumObject.Songs), "Songs is not empty");
                return View(albumDto);
            }
            if (_albumRepository.Any(x => x.Code.ToLower() == albumDto.Code.ToLower() && x.Id != id))
            {
                ModelState.AddModelError(nameof(AlbumObject.Code), "Code already used");
                return View(albumDto);
            }

            var album = _albumRepository.GetQueryable().Include(x => x.Songs).FirstOrDefault(x => x.Id == id);
            if (album == null)
                return RedirectToAction("Index");

            album.Code = albumDto.Code.ToLower();
            album.Name = albumDto.Name;
            album.Description = albumDto.Description;
            album.ReleaseDate = albumDto.ReleaseDate;

            if (album.Songs != null)
            {
                var removeSingers = new List<AlbumDetails>();

                //remove
                foreach (var item in album.Songs)
                    if (!albumDto.Songs.Contains(item.SongId))
                        removeSingers.Add(item);
                foreach (var item in removeSingers)
                    album.Songs.Remove(item);

                foreach (var item in albumDto.Songs)
                {
                    if (!album.Songs.Any(x => x.SongId == item))
                        album.Songs.Add(new AlbumDetails() { SongId = item, AlbumId = id });
                }
            }
            else
                album.Songs = albumDto.Songs.Select(x => new AlbumDetails() { SongId = x, AlbumId = id }).ToList();

            _albumRepository.Update(album);
            _albumRepository.SaveChange();
            return RedirectToAction("Index");
        }
    }
}