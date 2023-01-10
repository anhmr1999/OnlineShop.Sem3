using Shop.EntityFramework.Entities;
using Shop.EntityFramework.Infrastructures.Enums;
using Shop.EntityFramework.Infrastructures.Repository;
using Shop.Web.Common;
using Shop.Web.Common.ObjectRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Web.Areas.Admin.Controllers
{
    public class BillController : Controller

    {
        private readonly IRepository<Bill> _billRepository;
        public BillController(IRepository<Bill> billRepository)
        {
            _billRepository = billRepository;
        }
        // GET: Admin/Bill
        public ActionResult Index(BillFilter filter)
        {
            var query = _billRepository.GetQueryable()
              .WhereIf(!string.IsNullOrEmpty(filter.SearchKey), x => x.CustomerName.ToLower().Contains(filter.SearchKey))
              .WhereIf(filter.Status.HasValue, x => x.Status == filter.Status);
            var model = new CommonListResult<Bill>(); 
            model.Filter = filter;
            model.TotalCount = query.Count();
            model.TotalPage = Math.Ceiling((decimal)query.Count() / 5);
            model.List = query.OrderByDescending(x => x.CreationTime).PagedBy(filter,5).ToList();
            return View(model);
        }
        public ActionResult View(Guid id)
        {

            var bill = _billRepository.Get(id);
            if (bill == null)
                return RedirectToAction("Index");
            return Json(bill, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateStatus(BillEditObject req)
        {
            var bill = _billRepository.Get(req.Id);
            if (bill == null)
                return Json(new { success = false, message = "Đã có lỗi xảy ra, Vui lòng thử lại" });

            bill.Status =req.Status;
            _billRepository.Update(bill);
            _billRepository.SaveChange();
            return Json(new { success = true, message = "Cập nhật trạng thái thành công" }, JsonRequestBehavior.AllowGet);
        }
    }
}