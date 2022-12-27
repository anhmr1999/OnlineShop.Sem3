using Shop.EntityFramework.Entities;
using Shop.EntityFramework.Infrastructures.Repository;
using Shop.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Web.Areas.Admin.Controllers
{
    public class ActorOrSingerController : Controller
    {
        private readonly IRepository<ActorOrSinger> _actorRepository;

        public ActorOrSingerController(IRepository<ActorOrSinger> actorRepository)
        {
            _actorRepository = actorRepository;
        }

        // GET: Admin/ActorAndSinger
        public ActionResult Index(CommonFilter filter)
        {
            var query = _actorRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(filter.SearchKey), x => x.Code.ToLower().Contains(filter.SearchKey.ToLower()) || x.Name.ToLower().Contains(filter.SearchKey));
            var model = new CommonListResult<ActorOrSinger>();
            model.Filter = filter;
            model.TotalCount = query.Count();
            model.TotalPage = Math.Ceiling((decimal)query.Count() / 10);
            model.List = query.OrderByDescending(x => x.CreationTime).PagedBy(filter).ToList();
            return View(model);
        }

        public ActionResult Delete(Guid id)
        {
            var actor = _actorRepository.Get(id);
            if (actor == null)
                return RedirectToAction("Index");

            _actorRepository.Delete(actor);
            _actorRepository.SaveChange();
            return RedirectToAction("Index");
        }

        // GET: Admin/ActorAndSinger/Edit
        public ActionResult Edit(Guid id)
        {
            var actor = _actorRepository.Get(id);
            if (actor == null)
                return RedirectToAction("Index");

            return View(actor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, ActorOrSinger actor)
        {
            if (!ModelState.IsValid)
                return View(actor);

            if (_actorRepository.Any(x => x.Code.ToLower() == actor.Code.ToLower() && x.Id != id))
            {
                ModelState.AddModelError(nameof(Category.Code), $"code {actor.Code} has been used");
                return View(actor);
            }
            var act = _actorRepository.Get(id);
            if (act == null)
                return RedirectToAction("Index");

            act.Code = actor.Code.ToLower();
            act.Name = actor.Name;
            act.Dob = actor.Dob;
            act.Title = actor.Title;
            act.Description = actor.Description;

            _actorRepository.Update(act);
            _actorRepository.SaveChange();
            return RedirectToAction("Index");
        }

        // GET: Admin/ActorAndSinger/Add
        public ActionResult Add()
        {
            return View(new ActorOrSinger());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ActorOrSinger actor)
        {
            if (!ModelState.IsValid)
                return View(actor);

            if (_actorRepository.Any(x => x.Code.ToLower() == actor.Code.ToLower()))
            {
                ModelState.AddModelError(nameof(Category.Code), $"code {actor.Code} has been used");
                return View(actor);
            }

            actor.Code = actor.Code.ToLower();
            _actorRepository.Insert(actor);
            _actorRepository.SaveChange();
            return RedirectToAction("Index");

        }
        public ActionResult View(Guid id)
        {
            var actor = _actorRepository.Get(id);
            if (actor == null)
                return RedirectToAction("Index");
            return Json(actor,JsonRequestBehavior.AllowGet);
        }
    }
}