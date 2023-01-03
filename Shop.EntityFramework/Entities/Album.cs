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
    public class Album : FullAuditedEntity
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime? ReleaseDate { get; set; } // ngày phát hành
        public string Description { get; set; }

        public ICollection<AlbumDetails> Songs { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
