namespace Shop.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActorOrSingers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Dob = c.DateTime(),
                        Title = c.String(nullable: false),
                        Description = c.String(),
                        CreationTime = c.DateTime(nullable: false),
                        CreationUser = c.Guid(),
                        LastModifiedTime = c.DateTime(),
                        LastModifiedUser = c.Guid(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTime(),
                        DeletedUser = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SongAndSingers",
                c => new
                    {
                        SongId = c.Guid(nullable: false),
                        SingerId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.SongId, t.SingerId })
                .ForeignKey("dbo.ActorOrSingers", t => t.SingerId, cascadeDelete: true)
                .ForeignKey("dbo.SongOrTrailerOrGames", t => t.SongId, cascadeDelete: true)
                .Index(t => t.SongId)
                .Index(t => t.SingerId);
            
            CreateTable(
                "dbo.SongOrTrailerOrGames",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Image = c.String(nullable: false),
                        Type = c.Int(nullable: false),
                        ProducerId = c.Guid(),
                        CategoryId = c.Guid(nullable: false),
                        ManufactureDate = c.DateTime(),
                        PremiereDate = c.DateTime(),
                        AllowDownloadFree = c.Boolean(nullable: false),
                        Description = c.String(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        CreationUser = c.Guid(),
                        LastModifiedTime = c.DateTime(),
                        LastModifiedUser = c.Guid(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTime(),
                        DeletedUser = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Producers", t => t.ProducerId)
                .Index(t => t.ProducerId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.AlbumDetails",
                c => new
                    {
                        AbumId = c.Guid(nullable: false),
                        SongId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.AbumId, t.SongId })
                .ForeignKey("dbo.Albums", t => t.AbumId, cascadeDelete: true)
                .ForeignKey("dbo.SongOrTrailerOrGames", t => t.SongId, cascadeDelete: true)
                .Index(t => t.AbumId)
                .Index(t => t.SongId);
            
            CreateTable(
                "dbo.Albums",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        ReleaseDate = c.DateTime(),
                        Description = c.String(),
                        CreationTime = c.DateTime(nullable: false),
                        CreationUser = c.Guid(),
                        LastModifiedTime = c.DateTime(),
                        LastModifiedUser = c.Guid(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTime(),
                        DeletedUser = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AlbumId = c.Guid(nullable: false),
                        Price = c.Double(nullable: false),
                        SupplierId = c.Guid(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        CreationUser = c.Guid(),
                        LastModifiedTime = c.DateTime(),
                        LastModifiedUser = c.Guid(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTime(),
                        DeletedUser = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Albums", t => t.AlbumId, cascadeDelete: true)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId, cascadeDelete: true)
                .Index(t => t.AlbumId)
                .Index(t => t.SupplierId);
            
            CreateTable(
                "dbo.BillDetailts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        BillId = c.Guid(nullable: false),
                        ProductId = c.Guid(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bills", t => t.BillId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.BillId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Bills",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(),
                        CustomerName = c.String(),
                        CustomerPhone = c.String(),
                        CustomerEmail = c.String(),
                        CustomerAddress = c.String(),
                        Status = c.Int(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        CreationUser = c.Guid(),
                        LastModifiedTime = c.DateTime(),
                        LastModifiedUser = c.Guid(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTime(),
                        DeletedUser = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        CreationTime = c.DateTime(nullable: false),
                        CreationUser = c.Guid(),
                        LastModifiedTime = c.DateTime(),
                        LastModifiedUser = c.Guid(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTime(),
                        DeletedUser = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        CateFor = c.Boolean(),
                        Description = c.String(),
                        CreationTime = c.DateTime(nullable: false),
                        CreationUser = c.Guid(),
                        LastModifiedTime = c.DateTime(),
                        LastModifiedUser = c.Guid(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTime(),
                        DeletedUser = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Producers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        FoundingDate = c.DateTime(nullable: false),
                        Introduce = c.String(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        CreationUser = c.Guid(),
                        LastModifiedTime = c.DateTime(),
                        LastModifiedUser = c.Guid(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTime(),
                        DeletedUser = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        ShortDesciption = c.String(nullable: false),
                        Image = c.String(),
                        IsGlobal = c.Boolean(nullable: false),
                        Description = c.String(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        CreationUser = c.Guid(),
                        LastModifiedTime = c.DateTime(),
                        LastModifiedUser = c.Guid(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTime(),
                        DeletedUser = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PermissionGrants",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        ProviderName = c.String(),
                        ProviderKey = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        RoleName = c.String(),
                        IsDefault = c.Boolean(nullable: false),
                        IsStatic = c.Boolean(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        CreationUser = c.Guid(),
                        LastModifiedTime = c.DateTime(),
                        LastModifiedUser = c.Guid(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTime(),
                        DeletedUser = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        RoleId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Username = c.String(nullable: false),
                        SurName = c.String(),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Phone = c.String(),
                        Password = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        CreationUser = c.Guid(),
                        LastModifiedTime = c.DateTime(),
                        LastModifiedUser = c.Guid(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTime(),
                        DeletedUser = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Percent = c.Double(),
                        Price = c.Double(),
                        Quantity = c.Int(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Content = c.String(nullable: false),
                        Products = c.String(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        CreationUser = c.Guid(),
                        LastModifiedTime = c.DateTime(),
                        LastModifiedUser = c.Guid(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTime(),
                        DeletedUser = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.SongOrTrailerOrGames", "ProducerId", "dbo.Producers");
            DropForeignKey("dbo.SongOrTrailerOrGames", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.AlbumDetails", "SongId", "dbo.SongOrTrailerOrGames");
            DropForeignKey("dbo.AlbumDetails", "AbumId", "dbo.Albums");
            DropForeignKey("dbo.Products", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.BillDetailts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.BillDetailts", "BillId", "dbo.Bills");
            DropForeignKey("dbo.Products", "AlbumId", "dbo.Albums");
            DropForeignKey("dbo.SongAndSingers", "SongId", "dbo.SongOrTrailerOrGames");
            DropForeignKey("dbo.SongAndSingers", "SingerId", "dbo.ActorOrSingers");
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.BillDetailts", new[] { "ProductId" });
            DropIndex("dbo.BillDetailts", new[] { "BillId" });
            DropIndex("dbo.Products", new[] { "SupplierId" });
            DropIndex("dbo.Products", new[] { "AlbumId" });
            DropIndex("dbo.AlbumDetails", new[] { "SongId" });
            DropIndex("dbo.AlbumDetails", new[] { "AbumId" });
            DropIndex("dbo.SongOrTrailerOrGames", new[] { "CategoryId" });
            DropIndex("dbo.SongOrTrailerOrGames", new[] { "ProducerId" });
            DropIndex("dbo.SongAndSingers", new[] { "SingerId" });
            DropIndex("dbo.SongAndSingers", new[] { "SongId" });
            DropTable("dbo.Sales");
            DropTable("dbo.Users");
            DropTable("dbo.UserRoles");
            DropTable("dbo.Roles");
            DropTable("dbo.PermissionGrants");
            DropTable("dbo.News");
            DropTable("dbo.Producers");
            DropTable("dbo.Categories");
            DropTable("dbo.Suppliers");
            DropTable("dbo.Bills");
            DropTable("dbo.BillDetailts");
            DropTable("dbo.Products");
            DropTable("dbo.Albums");
            DropTable("dbo.AlbumDetails");
            DropTable("dbo.SongOrTrailerOrGames");
            DropTable("dbo.SongAndSingers");
            DropTable("dbo.ActorOrSingers");
        }
    }
}
