namespace Shop.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initdatabase : DbMigration
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
                "dbo.SongOrTrainerOrGames",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(nullable: false),
                        Name = c.String(nullable: false),
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
                        Producer_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Albums", t => t.AlbumId, cascadeDelete: true)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId, cascadeDelete: true)
                .ForeignKey("dbo.Producers", t => t.Producer_Id)
                .Index(t => t.AlbumId)
                .Index(t => t.SupplierId)
                .Index(t => t.Producer_Id);
            
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
                "dbo.AlbumDetails",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AbumId = c.Guid(nullable: false),
                        SongId = c.Guid(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        CreationUser = c.Guid(),
                        LastModifiedTime = c.DateTime(),
                        LastModifiedUser = c.Guid(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTime(),
                        DeletedUser = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Albums", t => t.AbumId, cascadeDelete: true)
                .ForeignKey("dbo.SongOrTrainerOrGames", t => t.SongId, cascadeDelete: true)
                .Index(t => t.AbumId)
                .Index(t => t.SongId);
            
            CreateTable(
                "dbo.BillDetailts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        BillId = c.Guid(nullable: false),
                        ProductId = c.Guid(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        CreationUser = c.Guid(),
                        LastModifiedTime = c.DateTime(),
                        LastModifiedUser = c.Guid(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTime(),
                        DeletedUser = c.Guid(),
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
            
            CreateTable(
                "dbo.SongOrTrainerOrGameActorOrSingers",
                c => new
                    {
                        SongOrTrainerOrGame_Id = c.Guid(nullable: false),
                        ActorOrSinger_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.SongOrTrainerOrGame_Id, t.ActorOrSinger_Id })
                .ForeignKey("dbo.SongOrTrainerOrGames", t => t.SongOrTrainerOrGame_Id, cascadeDelete: true)
                .ForeignKey("dbo.ActorOrSingers", t => t.ActorOrSinger_Id, cascadeDelete: true)
                .Index(t => t.SongOrTrainerOrGame_Id)
                .Index(t => t.ActorOrSinger_Id);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        User_Id = c.Guid(nullable: false),
                        Role_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Role_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.Role_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Role_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRoles", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.UserRoles", "User_Id", "dbo.Users");
            DropForeignKey("dbo.SongOrTrainerOrGames", "ProducerId", "dbo.Producers");
            DropForeignKey("dbo.Products", "Producer_Id", "dbo.Producers");
            DropForeignKey("dbo.Products", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.BillDetailts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.BillDetailts", "BillId", "dbo.Bills");
            DropForeignKey("dbo.Products", "AlbumId", "dbo.Albums");
            DropForeignKey("dbo.AlbumDetails", "SongId", "dbo.SongOrTrainerOrGames");
            DropForeignKey("dbo.AlbumDetails", "AbumId", "dbo.Albums");
            DropForeignKey("dbo.SongOrTrainerOrGames", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.SongOrTrainerOrGameActorOrSingers", "ActorOrSinger_Id", "dbo.ActorOrSingers");
            DropForeignKey("dbo.SongOrTrainerOrGameActorOrSingers", "SongOrTrainerOrGame_Id", "dbo.SongOrTrainerOrGames");
            DropIndex("dbo.UserRoles", new[] { "Role_Id" });
            DropIndex("dbo.UserRoles", new[] { "User_Id" });
            DropIndex("dbo.SongOrTrainerOrGameActorOrSingers", new[] { "ActorOrSinger_Id" });
            DropIndex("dbo.SongOrTrainerOrGameActorOrSingers", new[] { "SongOrTrainerOrGame_Id" });
            DropIndex("dbo.BillDetailts", new[] { "ProductId" });
            DropIndex("dbo.BillDetailts", new[] { "BillId" });
            DropIndex("dbo.AlbumDetails", new[] { "SongId" });
            DropIndex("dbo.AlbumDetails", new[] { "AbumId" });
            DropIndex("dbo.Products", new[] { "Producer_Id" });
            DropIndex("dbo.Products", new[] { "SupplierId" });
            DropIndex("dbo.Products", new[] { "AlbumId" });
            DropIndex("dbo.SongOrTrainerOrGames", new[] { "CategoryId" });
            DropIndex("dbo.SongOrTrainerOrGames", new[] { "ProducerId" });
            DropTable("dbo.UserRoles");
            DropTable("dbo.SongOrTrainerOrGameActorOrSingers");
            DropTable("dbo.Sales");
            DropTable("dbo.Users");
            DropTable("dbo.Roles");
            DropTable("dbo.PermissionGrants");
            DropTable("dbo.News");
            DropTable("dbo.Suppliers");
            DropTable("dbo.Bills");
            DropTable("dbo.BillDetailts");
            DropTable("dbo.AlbumDetails");
            DropTable("dbo.Albums");
            DropTable("dbo.Products");
            DropTable("dbo.Producers");
            DropTable("dbo.Categories");
            DropTable("dbo.SongOrTrainerOrGames");
            DropTable("dbo.ActorOrSingers");
        }
    }
}
