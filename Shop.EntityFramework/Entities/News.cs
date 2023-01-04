using Shop.EntityFramework.Infrastructures.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Shop.EntityFramework.Entities
{
    public class News : FullAuditedEntity
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ShortDesciption { get; set; }
        public string Image { get; set; }
        public bool IsGlobal { get; set; } // Tin quốc tế
        [Required]
        [AllowHtml]
        public string Description { get; set; }
    }
}
