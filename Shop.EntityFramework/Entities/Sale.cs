using Shop.EntityFramework.Infrastructures.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.EntityFramework.Entities
{
    public class Sale : FullAuditedEntity
    {
        public double? Percent { get; set; } // Phần trăm
        public double? Price { get; set; } // Giá
        public int? Quantity { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string Products { get; set; }
    }
}
