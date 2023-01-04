using Shop.EntityFramework.Entities;
using Shop.EntityFramework.Infrastructures.Permissions;
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
    public class RoleController : Controller
    {
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<PermissionGrant> _permissionGrantRepository;

        public RoleController(IRepository<Role> roleRepository, IRepository<PermissionGrant> permissionGrantRepository)
        {
            _roleRepository = roleRepository;
            _permissionGrantRepository = permissionGrantRepository;
        }

        // GET: Admin/Role
        public ActionResult Index(CommonFilter filter)
        {
            var query = _roleRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(filter.SearchKey), x => x.RoleName.ToLower().Contains(filter.SearchKey.ToLower()));

            var model = new CommonListResult<Role>()
            {
                Filter = filter,
                List = query.OrderByDescending(x => x.CreationTime).Skip((filter.PageIndex - 1) * 10).Take(10).ToList(),
                TotalCount = query.Count(),
                TotalPage = (int)Math.Ceiling((decimal)query.Count()/10)
            };
            return View(model);
        }

        public ActionResult Add()
        {
            return View(new RoleObject());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(RoleObject roleDto)
        {
            if(!ModelState.IsValid)
                return View(roleDto);

            if(_roleRepository.Any(x => x.RoleName == roleDto.RoleName))
            {
                ModelState.AddModelError(nameof(RoleObject.RoleName), "Role name is exists");
                return View(roleDto);
            }

            var role = new Role();
            role.RoleName= roleDto.RoleName;
            role.IsDefault= roleDto.IsDefault;
            role.IsStatic = roleDto.IsStatic;
            _roleRepository.Insert(role);
            _permissionGrantRepository.InsertMany(roleDto.Permissions.Select(x => new PermissionGrant() { Name = x, ProviderKey = role.RoleName, ProviderName = "R"}));
            _roleRepository.SaveChange();
            _permissionGrantRepository.SaveChange();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(Guid id)
        {
            var role = _roleRepository.Get(id);
            if (role == null)
                return RedirectToAction("Index");

            var dto = new RoleObject();
            dto.RoleName = role.RoleName;
            dto.IsDefault = role.IsDefault;
            dto.IsStatic = role.IsStatic;
            var permissions = _permissionGrantRepository.GetList(x => x.ProviderName == "R" && x.ProviderKey == role.RoleName);
            dto.Permissions = permissions.Select(x => x.Name).ToArray();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, RoleObject roleDto)
        {
            if (!ModelState.IsValid)
                return View(roleDto);

            var role = _roleRepository.Get(id);

            if(_roleRepository.Any(x => x.Id != id && x.RoleName == roleDto.RoleName))
            {
                ModelState.AddModelError(nameof(RoleObject.RoleName), "Role name is exists");
                return View(roleDto);
            }   
            
            role.RoleName = roleDto.RoleName;
            role.IsDefault = roleDto.IsDefault;
            role.IsStatic = roleDto.IsStatic;
            _roleRepository.Update(role);

            var permissions = _permissionGrantRepository.GetList(x => x.ProviderName == "R" && x.ProviderKey == role.RoleName);
            _permissionGrantRepository.DeleteMany(permissions);
            _permissionGrantRepository.InsertMany(roleDto.Permissions.Select(x => new PermissionGrant() { Name = x, ProviderKey = role.RoleName, ProviderName = "R" }));
            _roleRepository.SaveChange();
            _permissionGrantRepository.SaveChange();
            return RedirectToAction("Index");
        }
    }
}