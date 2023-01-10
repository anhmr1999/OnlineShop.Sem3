using Shop.EntityFramework.Entities;
using Shop.EntityFramework.Infrastructures.Permissions;
using Shop.EntityFramework.Infrastructures.Repository;
using Shop.Web.Authentications;
using Shop.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Web.Areas.Admin.Controllers
{
    public class ProducerController : Controller
    {
        private readonly IRepository<Producer> _producerRepository;

        public ProducerController(IRepository<Producer> producerRepository)
        {
            _producerRepository = producerRepository;
        }

        // GET: Admin/Producer
        [ShopAuthorize(Proxy = PermissionName.Producer)]
        public ActionResult Index(CommonFilter filter)
        {
            var query = _producerRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(filter.SearchKey), x => x.Code.ToLower().Contains(filter.SearchKey.ToLower()) || x.Name.ToLower().Contains(filter.SearchKey.ToLower()));

            var model = new CommonListResult<Producer>()
            {
                Filter= filter,
                List = query.OrderByDescending(x => x.CreationTime).PagedBy(filter).ToList(),
                TotalCount= query.Count(),
                TotalPage = (int) Math.Ceiling((decimal)query.Count()/10)
            };
            return View(model);
        }

        [ShopAuthorize(Proxy = PermissionName.ProducerAdd)]
        public ActionResult Add()
        {
            return View(new Producer() { FoundingDate = DateTime.Now});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ShopAuthorize(Proxy = PermissionName.ProducerAdd)]
        public ActionResult Add(Producer producer)
        {
            if(!ModelState.IsValid)
                return View(producer);

            _producerRepository.Insert(producer);
            _producerRepository.SaveChange();
            return RedirectToAction("Index");
        }

        [ShopAuthorize(Proxy = PermissionName.ProducerEdit)]
        public ActionResult Edit(Guid id)
        {
            var producer = _producerRepository.Get(id);
            if (producer == null)
                return RedirectToAction("Index");

            return View(producer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ShopAuthorize(Proxy = PermissionName.ProducerEdit)]
        public ActionResult Edit(Guid id, Producer producerDto)
        {
            if (!ModelState.IsValid)
                return View(producerDto);

            var producer = _producerRepository.Get(id);
            if (producer == null)
                return RedirectToAction("Index");

            if(_producerRepository.Any(x => x.Id != id && x.Code == producerDto.Code))
                return View(producerDto);

            producer.Code = producerDto.Code;
            producer.Name = producerDto.Name;
            producer.FoundingDate = producerDto.FoundingDate;
            producer.Introduce = producerDto.Introduce;
            _producerRepository.Update(producer);
            _producerRepository.SaveChange();
            return RedirectToAction("Index");
        }

        [ShopAuthorize(Proxy = PermissionName.ProducerDelete)]
        public ActionResult Delete(Guid id)
        {
            var producer = _producerRepository.Get(id);
            if (producer == null)
                return RedirectToAction("Index");

            _producerRepository.Delete(producer);
            _producerRepository.SaveChange();
            return RedirectToAction("Index");
        }
    }
}