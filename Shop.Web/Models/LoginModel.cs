using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Web.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Tài khoản đăng nhập không được bỏ trống")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Mật khẩu đăng nhập không được bỏ trống")]
        public string Password { get; set; }
        public string RememberMe { get; set; }
    }
}