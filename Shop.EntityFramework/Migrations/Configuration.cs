namespace Shop.EntityFramework.Migrations
{
    using Shop.EntityFramework.Common;
    using Shop.EntityFramework.Entities;
    using Shop.EntityFramework.Infrastructures.Permissions;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Shop.EntityFramework.ShopDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Shop.EntityFramework.ShopDbContext context)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Admin Manager",
                Username = "manager",
                Email = "manager@shop.com",
                Phone = "0123456789",
                Password = "1q2w3E*".ToMd5(),
                IsActive = true,
                CreationTime = DateTime.Now,
                Roles = new List<Role>()
            };
            var role = new Role()
            {
                Id = Guid.NewGuid(),
                RoleName = "manager",
                IsStatic = true,
                IsDefault = false,
                CreationTime = DateTime.Now
            };
            user.Roles.Add(role);
            var permissionGrants = GetPermssionGrant(PermissionProvider.Permissions);

            context.Roles.Add(role);
            context.PermissionGrants.AddRange(permissionGrants);
            context.SaveChanges();
            context.Users.Add(user);
            context.SaveChanges();
        }

        private ICollection<PermissionGrant> GetPermssionGrant(ICollection<Permission> permissions)
        {
            var result = new List<PermissionGrant>();
            foreach (var item in permissions)
            {
                result.Add(new PermissionGrant() { Id = Guid.NewGuid(), Name = item.Name, ProviderKey = "R", ProviderName = "manager" });
                if (item.Permissions != null && item.Permissions.Count > 0)
                    result.AddRange(GetPermssionGrant(item.Permissions));
            }
            return result;
        }
    }
}
