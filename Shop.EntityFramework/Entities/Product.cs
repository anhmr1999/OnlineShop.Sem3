using Shop.EntityFramework.Infrastructures.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Shop.EntityFramework.Entities
{
    public class Product : FullAuditedEntity
    {
        public Guid AlbumId { get; set; }
        [Required]
        public double Price { get; set; }
        public Guid SupplierId { get; set; }

        [ForeignKey("AlbumId")]
        public Album Album { get; set; }
        [ForeignKey("SupplierId")]
        public Supplier Supplier { get; set; }

        public ICollection<BillDetailt> BillDetails { get; set; }
    }
}
