using Shop.EntityFramework.Infrastructures.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.EntityFramework.Entities
{
    public class ActorOrSinger : FullAuditedEntity
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime? Dob { get; set; }
        [Required]
        public string Title { get; set; } // Chức danh
        public string Description { get; set; }
        public ICollection<SongOrTrailerOrGame> SongOrTrailerOrGames { get; set; }
    }
}
