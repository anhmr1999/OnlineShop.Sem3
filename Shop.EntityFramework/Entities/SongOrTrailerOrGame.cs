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
    public class SongOrTrailerOrGame : FullAuditedEntity
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Image { get; set; }
        public string Link { get; set; }
        public SongTrailerGameTypeEnum Type  { get; set; }
        public Guid? ProducerId { get; set; }
        public Guid CategoryId { get; set; }
        public DateTime? ManufactureDate { get; set; } // ngày sản xuất
        public DateTime? PremiereDate { get; set; } // ngày công chiếu
        [Required]
        public ICollection<SongAndSinger> ActorOrSingers { get; set; } // danh sách diễn viên
        public ICollection<AlbumDetails> Albums { get; set; } // danh sách diễn viên
        public bool AllowDownloadFree { get; set; }
        [Required]
        public string Description { get; set; }

        [ForeignKey("ProducerId")]
        public Producer Producer { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
