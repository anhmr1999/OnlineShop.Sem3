using Shop.EntityFramework.Entities;
using Shop.EntityFramework.Infrastructures.Repository;
using Shop.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Shop.Web.Common.ObjectRequests;
using Shop.EntityFramework.Infrastructures.Permissions;
using System.Security.Policy;
using System.Web.Helpers;
using System.Xml.Linq;

namespace Shop.Web.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<PermissionGrant> _permissionRepository;

        public UserController(IRepository<User> userRepository, IRepository<Role> roleRepository, IRepository<PermissionGrant> permissionRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
        }

        // GET: Admin/User
        public ActionResult Index(UserAdminFilter filter)
        {
            var query = _userRepository.GetQueryable().Include(x => x.Roles.Select(r => r.Role))
                .WhereIf(!string.IsNullOrEmpty(filter.SearchKey), x => x.Name.ToLower().Contains(filter.SearchKey.ToLower()) || x.SurName.ToLower().Contains(filter.SearchKey.ToLower())
                        || x.Phone.ToLower().Contains(filter.SearchKey.ToLower()) || x.Phone.ToLower().Contains(filter.SearchKey.ToLower()));

            var model = new CommonListResult<User>()
            {
                Filter = filter,
                List = query.OrderByDescending(x => x.CreationTime).PagedBy(filter).ToList(),
                TotalCount = query.Count(),
                TotalPage = (int)Math.Ceiling((decimal)query.Count() / 10)
            };
            if (filter.IsAdmin.HasValue)
            {
                model.List = model.List.Where(x => _permissionRepository.Any(p => p.ProviderKey == x.Id.ToString() && p.ProviderName == "U" && p.Name == PermissionName.Admin) == filter.IsAdmin).ToList();
                model.TotalCount = model.List.Count();
                model.TotalPage = (int)Math.Ceiling((decimal)model.TotalCount / 10);
            }
            return View(model);
        }

        // GET: Admin/User/Add
        public ActionResult Add()
        {
            ViewBag.Roles = _roleRepository.GetQueryable().ToList();
            return View(new UserCoUObject());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(UserCoUObject userObj)
        {
            ViewBag.Roles = _roleRepository.GetQueryable().ToList();
            if (!ModelState.IsValid)
                return View(userObj);

            if (!userObj.Email.ValidateEmail())
            {
                ModelState.AddModelError(nameof(UserCoUObject.Email), "Email is not correct");
                return View(userObj);
            }
            if (!string.IsNullOrEmpty(userObj.Phone) && !userObj.Phone.ValidatePhoneNumber())
            {
                ModelState.AddModelError(nameof(UserCoUObject.Email), "Phone number is not correct");
                return View(userObj);
            }
            var user = new User()
            {
                Id = Guid.NewGuid(),
                SurName = userObj.SurName,
                Name = userObj.Name,
                Email = userObj.Email,
                Username = userObj.Email,
                Phone = userObj.Phone,
                IsActive = userObj.IsActive,
                Password = userObj.Password.ToMd5(),
                Roles = new List<UserRole>()
            };
            if (userObj.RoleId.HasValue)
            {
                var role = _roleRepository.Get(userObj.RoleId.Value);
                if (role == null)
                {
                    ModelState.AddModelError(nameof(UserCoUObject.RoleId), "Role is not correct");
                    return View(userObj);
                }
                user.Roles.Add(new UserRole() { RoleId = role.Id, UserId = user.Id});
            }
            _userRepository.Insert(user);
            _userRepository.SaveChange();
            _permissionRepository.Insert(new PermissionGrant() { Name = PermissionName.Admin, ProviderKey = user.Id.ToString(), ProviderName = "U" });
            _permissionRepository.SaveChange();
            return RedirectToAction("Index");
        }

        // GET: Admin/User/Edit
        public ActionResult Edit(Guid id)
        {
            var user = _userRepository.GetQueryable().Include(x => x.Roles).FirstOrDefault(x => x.Id == id);
            if (user == null)
                return RedirectToAction("Index");

            var userDto = new UserCoUObject()
            {
                Id = id,
                SurName = user.SurName,
                Name = user.Name,
                Phone = user.Phone,
                Email = user.Email,
                IsActive = user.IsActive,
                RoleId = user.Roles.FirstOrDefault()?.RoleId,
                Password = "●●●●●●●●●●"
            };
            ViewBag.Roles = _roleRepository.GetQueryable().ToList();
            return View(userDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, UserCoUObject userObj)
        {
            ViewBag.Roles = _roleRepository.GetQueryable().ToList();
            if (!ModelState.IsValid)
                return View(userObj);

            if (!userObj.Email.ValidateEmail())
            {
                ModelState.AddModelError(nameof(UserCoUObject.Email), "Email is not correct");
                return View(userObj);
            }
            if (!string.IsNullOrEmpty(userObj.Phone) && !userObj.Phone.ValidatePhoneNumber())
            {
                ModelState.AddModelError(nameof(UserCoUObject.Email), "Phone number is not correct");
                return View(userObj);
            }

            var user = _userRepository.GetQueryable().Include(x => x.Roles).FirstOrDefault(x => x.Id == id);
            if (user == null)
                return RedirectToAction("Index");

            user.SurName = userObj.SurName;
            user.Name = userObj.Name;
            user.Email = userObj.Email;
            user.Username = userObj.Email;
            user.Phone = userObj.Phone;
            user.IsActive = userObj.IsActive;

            if (user.Roles != null)
            {
                if(!userObj.RoleId.HasValue || !user.Roles.Any(x => x.RoleId == userObj.RoleId))
                {
                    user.Roles.Remove(user.Roles.FirstOrDefault());
                    if (userObj.RoleId.HasValue)
                        user.Roles.Add(new UserRole() { UserId = id, RoleId = userObj.RoleId.Value });
                }    
            }
            else
            {
                if(userObj.RoleId.HasValue)
                    user.Roles = new List<UserRole>() { new UserRole() { UserId = id, RoleId = userObj.RoleId.Value } };
            }    

            _userRepository.Update(user);
            _userRepository.SaveChange();
            return RedirectToAction("Index");
        }
    }
}