using System;
using System.Collections.Generic;

namespace Shop.EntityFramework.Infrastructures.Permissions
{
    public static class PermissionProvider
    {
        public static List<Permission> Permissions = new List<Permission>()
        {
            new Permission(PermissionName.Category, "Quản lý danh mục", permissions: new List<Permission>(){
                new Permission(PermissionName.CategoryAdd, "Thêm mới danh mục", PermissionName.Category),
                new Permission(PermissionName.CategoryEdit, "Chỉnh sửa danh mục", PermissionName.Category),
                new Permission(PermissionName.CategoryDelete, "Xóa danh mục", PermissionName.Category),
            }),
            new Permission(PermissionName.ActorOrSinger, "Quản lý nghệ sĩ", permissions: new List<Permission>(){
                new Permission(PermissionName.ActorOrSingerAdd, "Thêm mới nghệ sĩ", PermissionName.ActorOrSinger),
                new Permission(PermissionName.ActorOrSingerEdit, "Chỉnh sửa nghệ sĩ", PermissionName.ActorOrSinger),
                new Permission(PermissionName.ActorOrSingerDelete, "Xóa nghệ sĩ", PermissionName.ActorOrSinger),
            }),
            new Permission(PermissionName.Album, "Quản lý Album", permissions: new List<Permission>(){
                new Permission(PermissionName.AlbumAdd, "Thêm mới Album", PermissionName.Album),
                new Permission(PermissionName.AlbumEdit, "Chỉnh sửa Album", PermissionName.Album),
                new Permission(PermissionName.AlbumDelete, "Xóa Album", PermissionName.Album),
            }),
            new Permission(PermissionName.SongTrainerGame, "Quản lý bài hát, trainer, game", permissions: new List<Permission>(){
                new Permission(PermissionName.SongTrainerGameAdd, "Thêm mới bài hát, trainer, game", PermissionName.SongTrainerGame),
                new Permission(PermissionName.SongTrainerGameEdit, "Chỉnh sửa bài hát, trainer, game", PermissionName.SongTrainerGame),
                new Permission(PermissionName.SongTrainerGameDelete, "Xóa bài hát, trainer, game", PermissionName.SongTrainerGame),
            }),
            new Permission(PermissionName.User, "Quản lý tài khoản", permissions: new List<Permission>(){
                new Permission(PermissionName.UserAdd, "Thêm mới tài khoản", PermissionName.User),
                new Permission(PermissionName.UserEdit, "Chỉnh sửa tài khoản", PermissionName.User),
                new Permission(PermissionName.UserDelete, "Xóa tài khoản", PermissionName.User),
                new Permission(PermissionName.UserRestore, "Khôi phục tài khoản", PermissionName.User),
            }),
            new Permission(PermissionName.Producer, "Quản lý nhà sản xuất", permissions: new List<Permission>(){
                new Permission(PermissionName.ProducerAdd, "Thêm nhà sản xuất", PermissionName.Producer),
                new Permission(PermissionName.ProducerEdit, "Chỉnh sửa nhà sản xuất", PermissionName.Producer),
                new Permission(PermissionName.ProducerDelete, "Xóa nhà sản xuất", PermissionName.Producer),
            }),
            new Permission(PermissionName.Supplier, "Quản lý nhà cung cấp", permissions: new List<Permission>(){
                new Permission(PermissionName.SupplierAdd, "Thêm mới nhà cung cấp", PermissionName.Supplier),
                new Permission(PermissionName.SupplierEdit, "Chỉnh sửa nhà cung cấp", PermissionName.Supplier),
                new Permission(PermissionName.SupplierDelete, "Xóa nhà cung cấp", PermissionName.Supplier),
            }),
            new Permission(PermissionName.Role, "Quản lý vai trò", permissions: new List<Permission>(){
                new Permission(PermissionName.RoleAdd, "Thêm mới vai trò", PermissionName.Role),
                new Permission(PermissionName.RoleEdit, "Chỉnh sửa vai trò", PermissionName.Role),
                new Permission(PermissionName.RoleDelete, "Xóa vai trò", PermissionName.Role),
            }),
            new Permission(PermissionName.Product, "Quản lý sản phẩm", permissions: new List<Permission>(){
                new Permission(PermissionName.ProductAdd, "Thêm mới sản phẩm", PermissionName.Product),
                new Permission(PermissionName.ProductEdit, "Chỉnh sửa sản phẩm", PermissionName.Product),
                new Permission(PermissionName.ProductDelete, "Xóa sản phẩm", PermissionName.Product),
            }),
            new Permission(PermissionName.Bill, "Quản lý hóa đơn", permissions: new List<Permission>(){
                new Permission(PermissionName.BillAdd, "Thêm mới hóa đơn", PermissionName.Bill),
                new Permission(PermissionName.BillEdit, "Chỉnh sửa hóa đơn", PermissionName.Bill),
                new Permission(PermissionName.BillDelete, "Xóa hóa đơn", PermissionName.Bill),
            }),
            new Permission(PermissionName.News, "Quản lý tin tức", permissions: new List<Permission>(){
                new Permission(PermissionName.NewsAdd, "Thêm mới tin tức", PermissionName.News),
                new Permission(PermissionName.NewsEdit, "Chỉnh sửa tin tức", PermissionName.News),
                new Permission(PermissionName.NewsDelete, "Xóa tin tức", PermissionName.News),
            }),
            new Permission(PermissionName.Sale, "Quản lý khuyến mãi", permissions: new List<Permission>(){
                new Permission(PermissionName.SaleAdd, "Thêm mới khuyến mãi", PermissionName.Sale),
                new Permission(PermissionName.SaleEdit, "Chỉnh sửa khuyến mãi", PermissionName.Sale),
                new Permission(PermissionName.SaleDelete, "Xóa khuyến mãi", PermissionName.Sale),
            }),
        };
    }

    public static class PermissionName
    {
        public const string Category = "Category";
        public const string CategoryAdd = "Category.Add";
        public const string CategoryEdit = "Category.Edit";
        public const string CategoryDelete = "Category.Delete";

        public const string ActorOrSinger = "ActorOrSinger";
        public const string ActorOrSingerAdd = "ActorOrSinger.Add";
        public const string ActorOrSingerEdit = "ActorOrSinger.Edit";
        public const string ActorOrSingerDelete = "ActorOrSinger.Delete";

        public const string Album = "Album";
        public const string AlbumAdd = "Album.Add";
        public const string AlbumEdit = "Album.Edit";
        public const string AlbumDelete = "Album.Delete";

        public const string SongTrainerGame = "SongTrainerGame";
        public const string SongTrainerGameAdd = "SongTrainerGame.Add";
        public const string SongTrainerGameEdit = "SongTrainerGame.Edit";
        public const string SongTrainerGameDelete = "SongTrainerGame.Delete";

        public const string User = "User";
        public const string UserAdd = "User.Add";
        public const string UserEdit = "User.Edit";
        public const string UserDelete = "User.Delete";
        public const string UserRestore = "User.Restore";

        public const string Producer = "Producer";
        public const string ProducerAdd = "Producer.Add";
        public const string ProducerEdit = "Producer.Edit";
        public const string ProducerDelete = "Producer.Delete";

        public const string Supplier = "Supplier";
        public const string SupplierAdd = "Supplier.Add";
        public const string SupplierEdit = "Supplier.Edit";
        public const string SupplierDelete = "Supplier.Delete";

        public const string Role = "Role";
        public const string RoleAdd = "Role.Add";
        public const string RoleEdit = "Role.Edit";
        public const string RoleDelete = "Role.Delete";

        public const string Product = "Product";
        public const string ProductAdd = "Product.Add";
        public const string ProductEdit = "Product.Edit";
        public const string ProductDelete = "Product.Delete";

        public const string Bill = "Bill";
        public const string BillAdd = "Bill.Add";
        public const string BillEdit = "Bill.Edit";
        public const string BillDelete = "Bill.Delete";

        public const string News = "News";
        public const string NewsAdd = "News.Add";
        public const string NewsEdit = "News.Edit";
        public const string NewsDelete = "News.Delete";

        public const string Sale = "Sale";
        public const string SaleAdd = "Sale.Add";
        public const string SaleEdit = "Sale.Edit";
        public const string SaleDelete = "Sale.Delete";

        public const string Admin = "admin";
    }
}
