using Shop.EntityFramework.Infrastructures.Entities.Auditing;
using Shop.EntityFramework.Infrastructures.Enums;
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
    public class Bill : FullAuditedEntity
    {
        public Guid? UserId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
        public BillStatusEnum Status { get; set; }

        public ICollection<BillDetailt> Details { get; set; }
    }

    public class BillDetailt : Entity
    {
        public Guid BillId { get; set; }
        public Guid ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double Price { get; set; }

        [ForeignKey("BillId")]
        public Bill Bill { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
