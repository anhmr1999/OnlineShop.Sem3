using Shop.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
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
        public DbSet<SongOrTrailerOrGame> SongOrTrailerOrGames { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<SongAndSinger> SongAndSingers { get; set; }
        public DbSet<AlbumDetails> AlbumDetails { get; set; }

        public ShopDbContext() : base("shopConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserRole>().HasKey(x => new { x.RoleId, x.UserId });
            modelBuilder.Entity<SongAndSinger>().HasKey(x => new { x.SongId, x.SingerId });
            modelBuilder.Entity<AlbumDetails>().HasKey(x => new { x.AbumId, x.SongId });
        }
    }
}
