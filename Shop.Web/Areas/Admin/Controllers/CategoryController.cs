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
using System.Web.Routing;

namespace Shop.Web.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryController(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: Admin/Category
        [ShopAuthorize(Proxy = PermissionName.Category)]
        public ActionResult Index(CategoryAdminFilter filter)
        {
            var query = _categoryRepository.GetQueryable()
                //.Where(x => x.CateFor == filter.CateFor)
                .Where( x=> x.CateFor==filter.CateFor)
                .WhereIf(!string.IsNullOrEmpty(filter.SearchKey), x => x.Code.ToLower().Contains(filter.SearchKey) || x.Name.ToLower().Contains(filter.SearchKey) || x.Description.ToLower().Contains(filter.SearchKey));
            var model = new CommonListResult<Category>();
            model.Filter = filter;
            model.TotalCount = query.Count();
            model.TotalPage = Math.Ceiling((decimal)model.TotalCount / 10);
            model.List = query.OrderByDescending(x => x.CreationTime).PagedBy(filter).ToList();
            return View(model);
        }
        // GET: Admin/Category/Add
        [ShopAuthorize(Proxy = PermissionName.CategoryAdd)]
        public ActionResult Add()
        {
            return View(new Category());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ShopAuthorize(Proxy = PermissionName.CategoryAdd)]
        public ActionResult Add(Category category)
        {
            if(!ModelState.IsValid)
                return View(category);

            if (_categoryRepository.Any(x => x.Code.ToLower() == category.Code.ToLower()))
            {
                ModelState.AddModelError(nameof(Category.Code), $"code {category.Code} has been used");
                return View(category);
            }

            category.Code = category.Code.ToLower();
            _categoryRepository.Insert(category);
            _categoryRepository.SaveChange();
            return RedirectToAction("Index");
        }

        [ShopAuthorize(Proxy = PermissionName.CategoryDelete)]
        public ActionResult Delete(Guid id)
        {
            var cate = _categoryRepository.Get(id);
            if (cate == null)
                return RedirectToAction("Index");

            _categoryRepository.Delete(cate);
            _categoryRepository.SaveChange();
            return RedirectToAction("Index");
        }


        // GET: Admin/Category/Edit
        [ShopAuthorize(Proxy = PermissionName.CategoryEdit)]
        public ActionResult Edit(Guid id)
        {
            var category = _categoryRepository.Get(id);
            if (category == null)
                return RedirectToAction("Index");
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ShopAuthorize(Proxy = PermissionName.CategoryEdit)]
        public ActionResult Edit(Guid id, Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            if (_categoryRepository.Any(x => x.Code.ToLower() == category.Code.ToLower() && x.Id != id))
            {
                ModelState.AddModelError(nameof(Category.Code), $"code {category.Code} has been used");
                return View(category);
            }
            var cate = _categoryRepository.Get(id);
            if(cate == null)
                return RedirectToAction("Index");

            cate.Code = category.Code.ToLower();
            cate.Name = category.Name;
            cate.CateFor = category.CateFor;
            cate.Description = category.Description;

            _categoryRepository.Update(cate);
            _categoryRepository.SaveChange();
            return RedirectToAction("Index");
        }

        [ShopAuthorize(Proxy = PermissionName.Category)]
        public ActionResult View(Guid id)
        {
           
            var cate = _categoryRepository.Get(id);
            if (cate == null)
                return RedirectToAction("Index");
            return Json(cate,JsonRequestBehavior.AllowGet);
        }

    }
}