using Shop.EntityFramework.Entities;
using Shop.EntityFramework.Infrastructures.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Web.Controllers
{
    public class MusicController : Controller
    {
        private readonly IRepository<SongOrTrailerOrGame> _songRepository;

        public MusicController(IRepository<SongOrTrailerOrGame> songRepository)
        {
            _songRepository = songRepository;
        }


        // GET: Music
        public ActionResult Index()
        {
            return View();
        }

        // GET: Music Detail
        public ActionResult Views(string name)
        {
            return View();
        }
        // VIEW 
        public ActionResult GetAll()
        {
            var sale = _songRepository.GetQueryable();
            if (sale == null)
                return RedirectToAction("Index");
            return Json(sale, JsonRequestBehavior.AllowGet);
        }
    }
}