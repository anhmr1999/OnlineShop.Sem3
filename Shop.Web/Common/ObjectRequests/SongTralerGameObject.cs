using Shop.EntityFramework.Entities;
using Shop.EntityFramework.Infrastructures.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Web.Common.ObjectRequests
{
    public class SongTralerGameObject
    {
        [Required(ErrorMessage = "Code is not empty!")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Name is not empty!")]
        public string Name { get; set; }
        public string Image { get; set; }
        public Guid? ProducerId { get; set; }
        [Required(ErrorMessage = "Category is not empty!")]
        public Guid CategoryId { get; set; }
        public DateTime? ManufactureDate { get; set; } // ngày sản xuất
        public DateTime? PremiereDate { get; set; } // ngày công chiếu
        [Required(ErrorMessage = "Actor or singner is not empty!")]
        public string ActorOrSingers { get; set; } // danh sách diễn viên
        public bool AllowDownloadFree { get; set; }
        [Required(ErrorMessage = "Description is not empty!")]
        [AllowHtml]
        public string Description { get; set; }
    }
}