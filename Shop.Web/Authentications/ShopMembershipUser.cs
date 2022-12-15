using Shop.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Web.Security;

namespace Shop.Web.Authentications
{
    public class ShopMembershipUser : MembershipUser
    {
        public Guid? Id { get; set; }
        public string SurName { get; set; }
        public string Name { get; set; }
        public ICollection<Role> Roles { get; set; }
        public ICollection<PermissionGrant> Permissions { get; set; }

        public ShopMembershipUser(User user, ICollection<PermissionGrant> permissions) : base (nameof(ShopMembershipUser), user.Username, user.Id, user.Email, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            Id = user.Id;
            SurName = user.SurName;
            Name = user.Name;
            Roles = user.Roles;
            Permissions = permissions;
        }
    }
}