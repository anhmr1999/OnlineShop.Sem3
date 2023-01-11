using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace Shop.Web.Models
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "Email required")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone required")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Address required")]
        public string Address { get; set; }

        public string SurName { get; set; }

        [Required(ErrorMessage = "Name required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password required")]
        [Compare("Password", ErrorMessage = "Confirm password does not match with password")]
        public string ConfirmPassword { get; set; }
    }
}