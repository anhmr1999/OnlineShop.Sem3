using Shop.EntityFramework.Entities;
using Shop.EntityFramework.Infrastructures;
using Shop.EntityFramework.Infrastructures.Enums;
using Shop.EntityFramework.Infrastructures.Repository;
using Shop.Web.Common.ObjectRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IRepository<Bill> _billRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Sale> _saleRepository;
        private readonly IRepository<User> _userRepository;
        private readonly ICurrentPrincipal _currentPrincipal;

        public CartController(IRepository<Bill> billRepository, IRepository<Product> productRepository, IRepository<Sale> saleRepository, IRepository<User> userRepository, ICurrentPrincipal currentPrincipal)
        {
            _billRepository = billRepository;
            _productRepository = productRepository;
            _saleRepository = saleRepository;
            _userRepository = userRepository;
            _currentPrincipal = currentPrincipal;
        }

        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SaveBill(BillObject[] bills)
        {
            var products = _productRepository.GetList(x => bills.Select(p => p.Code).ToList().Contains(x.Code));
            if(products.Count == 0)
                return Json(new { success = false, message = "Không tìm thấy sản phẩm, Vui lòng thử lại" });

            var sales = _saleRepository.GetList(x => x.StartDate < DateTime.Now && DateTime.Now < x.EndDate);
            var user = _userRepository.Get(_currentPrincipal.CurrentUserId.Value);
            var bill = new Bill()
            {
                UserId = user.Id,
                CustomerName = user.Name,
                CustomerEmail = user.Email,
                CustomerPhone = user.Phone,
                CustomerAddress = user.Address,
                Status = BillStatusEnum.Created,
                Details = new List<BillDetailt>()
            };
            foreach (var item in products)
            {
                var sale = sales.FirstOrDefault(x => x.Products.Contains(item.Id.ToString()));
                var detail = new BillDetailt()
                {
                    Id = Guid.NewGuid(),
                    ProductId = item.Id,
                    Quantity = bills.FirstOrDefault(x => x.Code == item.Code).Quantity
                };
                if (sale == null)
                    detail.Price = item.Price;
                else
                {
                    var price = detail.Price;
                    if (sale.Percent.HasValue)
                        price = price - (price * sale.Percent.Value) / 100;
                    else if (sale.Price.HasValue)
                        price = sale.Price.Value;

                    detail.Price = price;
                }
                bill.Details.Add(detail);
            }
            try
            {
                _billRepository.Insert(bill);
                _billRepository.SaveChange();
                return Json(new { success = true, message = "Lưu hóa đơn thành công" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Đã có lỗi xảy ra, Vui lòng thử lại" });
            }
        }
    }
}