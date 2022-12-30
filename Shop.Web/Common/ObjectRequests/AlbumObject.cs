using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Web.Common.ObjectRequests
{
    public class AlbumObject
    {
        [Required(ErrorMessage = "Code is not empty")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Code is not empty")]
        public string Name { get; set; }
        public DateTime? ReleaseDate { get; set; } // ngày phát hành
        [Required(ErrorMessage = "Code is not empty")]
        public string Description { get; set; }
        public Guid[] Songs { get; set; }
    }
}