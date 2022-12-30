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
        public static string Category = "Category";
        public static string CategoryAdd = "Category.Add";
        public static string CategoryEdit = "Category.Edit";
        public static string CategoryDelete = "Category.Delete";

        public static string ActorOrSinger = "ActorOrSinger";
        public static string ActorOrSingerAdd = "ActorOrSinger.Add";
        public static string ActorOrSingerEdit = "ActorOrSinger.Edit";
        public static string ActorOrSingerDelete = "ActorOrSinger.Delete";

        public static string Album = "Album";
        public static string AlbumAdd = "Album.Add";
        public static string AlbumEdit = "Album.Edit";
        public static string AlbumDelete = "Album.Delete";

        public static string SongTrainerGame = "SongTrainerGame";
        public static string SongTrainerGameAdd = "SongTrainerGame.Add";
        public static string SongTrainerGameEdit = "SongTrainerGame.Edit";
        public static string SongTrainerGameDelete = "SongTrainerGame.Delete";

        public static string User = "User";
        public static string UserAdd = "User.Add";
        public static string UserEdit = "User.Edit";
        public static string UserDelete = "User.Delete";
        public static string UserRestore = "User.Restore";

        public static string Producer = "Producer";
        public static string ProducerAdd = "Producer.Add";
        public static string ProducerEdit = "Producer.Edit";
        public static string ProducerDelete = "Producer.Delete";

        public static string Supplier = "Supplier";
        public static string SupplierAdd = "Supplier.Add";
        public static string SupplierEdit = "Supplier.Edit";
        public static string SupplierDelete = "Supplier.Delete";

        public static string Role = "Role";
        public static string RoleAdd = "Role.Add";
        public static string RoleEdit = "Role.Edit";
        public static string RoleDelete = "Role.Delete";

        public static string Product = "Product";
        public static string ProductAdd = "Product.Add";
        public static string ProductEdit = "Product.Edit";
        public static string ProductDelete = "Product.Delete";

        public static string Bill = "Bill";
        public static string BillAdd = "Bill.Add";
        public static string BillEdit = "Bill.Edit";
        public static string BillDelete = "Bill.Delete";

        public static string News = "News";
        public static string NewsAdd = "News.Add";
        public static string NewsEdit = "News.Edit";
        public static string NewsDelete = "News.Delete";

        public static string Sale = "Sale";
        public static string SaleAdd = "Sale.Add";
        public static string SaleEdit = "Sale.Edit";
        public static string SaleDelete = "Sale.Delete";

        public static string Admin = "admin";
    }
}
