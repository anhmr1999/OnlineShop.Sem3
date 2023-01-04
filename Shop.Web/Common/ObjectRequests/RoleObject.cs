using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Web.Common.ObjectRequests
{
    public class RoleObject
    {
        [Required(ErrorMessage = "Role name is not empty!")]
        public string RoleName { get; set; }
        public bool IsDefault { get; set; }
        public bool IsStatic { get; set; }
        public string[] Permissions { get; set; } = new string[0];
    }
}