using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Web.Common.ObjectRequests
{
    public class UserCoUObject
    {
        public Guid? Id { get; set; }
        public string Image { get; set; }
        public string SurName { get; set; }
        [Required(ErrorMessage = "Name cannot be empty!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email cannot be empty!")]
        public string Email { get; set; }
        public string Phone { get; set; }
        [Required(ErrorMessage = "Password cannot be empty!")]
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public Guid? RoleId { get; set; }
    }
}