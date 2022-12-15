using Shop.EntityFramework.Entities;
using Shop.EntityFramework.Infrastructures.Permissions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.EntityFramework
{
    public class ShopDbContextInitializer : DropCreateDatabaseAlways<ShopDbContext>
    {
        protected override void Seed(ShopDbContext context)
        {
            context.Roles.Add(new Role()
            {
                Id = Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e"),
                RoleName = "admin",
                IsDefault = false,
                IsStatic = true,
                CreationTime = DateTime.Now
            });

            context.Users.Add(new User()
            {
                Id = Guid.Parse("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                Username = "admin",
                Name = "admin",
                Email = "admin@shop.com",
                Password = "",
                Roles = new List<Role>() { new Role() { Id = Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e"), RoleName = "admin", IsDefault = false, IsStatic = true, } }
            });

            context.PermissionGrants.AddRange(PermissionProvider.Permissions.Select(x => new PermissionGrant() { Id = Guid.NewGuid(), Name = x.Name, ProviderKey = "R", ProviderName = "7c9e6679-7425-40de-944b-e07fc1f90ae7" }));
            context.SaveChanges();

            base.Seed(context);
        }
    }
}
