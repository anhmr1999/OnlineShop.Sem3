using Shop.EntityFramework.Infrastructures.Entities.Auditing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.EntityFramework.Entities
{
    public class Category : FullAuditedEntity
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public bool? CateFor { get; set; } // null trainer, true song, false game
        public string Description { get; set; }

        public ICollection<SongOrTrailerOrGame> SongOrTrailerOrGames { get; set; }
    }
}
