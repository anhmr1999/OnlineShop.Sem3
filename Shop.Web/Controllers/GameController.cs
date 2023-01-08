using Shop.EntityFramework.Entities;
using Shop.EntityFramework.Infrastructures.Enums;
using Shop.EntityFramework.Infrastructures.Repository;
using Shop.Web.Common;
using Shop.Web.Common.PageFilter;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Web.Controllers
{
    public class GameController : Controller
    {
        private readonly IRepository<SongOrTrailerOrGame> _songRepository;

        public GameController(IRepository<SongOrTrailerOrGame> songRepository)
        {
            _songRepository = songRepository;
        }


        // GET: Music
        public ActionResult Index(CommonPageFilter filter)
        {
            var query = _songRepository.GetQueryable().Include(x => x.Category)
                .Where(x => x.Type == SongTrailerGameTypeEnum.Game)
                .WhereIf(!string.IsNullOrEmpty(filter.SearchKey), x => x.Name.ToLower().Contains(filter.SearchKey.ToLower()));
            if (string.IsNullOrEmpty(filter.Order))
                query = query.OrderBy(x => x.Code);
            else if (filter.Order == "desc")
                query = query.OrderByDescending(x => x.CreationTime);
            else
                query = query.OrderBy(x => x.CreationTime);

            var paged = query.PagedBy(filter).ToList();
            var model = new CommonPageResult<SongOrTrailerOrGame>()
            {
                Data = paged,
                Filter = filter,
                TotalPage = (int)Math.Ceiling((decimal)query.Count() / 12)
            };

            return View(model);
        }

        public ActionResult View(string code)
        {
            var song = _songRepository.GetQueryable().Include(x => x.Category)
                .Include(x => x.ActorOrSingers.Select(s => s.Singer))
                .FirstOrDefault(x => x.Code == code);
            if (song == null)
                return RedirectToAction("Index");

            var likeSong = _songRepository.GetQueryable().Include(x => x.Category)
                .Where(x => x.Type == SongTrailerGameTypeEnum.Game)
                .Where(x => x.Id != song.Id).OrderByDescending(x => x.CreationTime).Take(4).ToList();
            var model = new ViewDetailt<SongOrTrailerOrGame>()
            {
                Data = song,
                LikeData = likeSong
            };
            return View(model);
        }
    }
}