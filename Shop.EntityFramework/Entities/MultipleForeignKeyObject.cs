using Shop.EntityFramework.Infrastructures.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.EntityFramework.Entities
{
    public class UserRole
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid RoleId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }

    public class SongAndSinger
    {
        [Required]
        public Guid SongId { get; set; }
        [Required]
        public Guid SingerId { get; set; }

        [ForeignKey("SongId")]
        public SongOrTrailerOrGame Song { get; set; }
        [ForeignKey("SingerId")]
        public ActorOrSinger Singer { get; set; }
    }

    public class AlbumDetails
    {
        [Required]
        public Guid AlbumId { get; set; }
        [Required]
        public Guid SongId { get; set; }

        [ForeignKey("AlbumId")]
        public Album Album { get; set; }
        [ForeignKey("SongId")]
        public SongOrTrailerOrGame Song { get; set; }
    }
}
