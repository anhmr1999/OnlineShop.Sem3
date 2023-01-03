using Shop.EntityFramework.Infrastructures.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.EntityFramework.Entities
{
    public class Producer : FullAuditedEntity
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime? FoundingDate { get; set; } // ngày thành lập
        [Required]
        public string Introduce { get; set; } // giới thiệu công ty
    }
}
