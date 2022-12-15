using Shop.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.EntityFramework
{
    public class ShopDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<ActorOrSinger> ActorOrSingers { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillDetailt> BillDetailts { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<PermissionGrant> PermissionGrants { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SongOrTrainerOrGame> SongOrTrainerOrGames { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<User> Users { get; set; }

        public ShopDbContext() : base("shopConnection")
        {}
    }
}
