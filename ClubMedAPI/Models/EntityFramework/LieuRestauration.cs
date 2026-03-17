using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("lieurestauration")]
    public class LieuRestauration
    {
        [Key]
        [Column("numrestauration")]
        public int NumRestauration { get; set; }

        [Column("numphoto")]
        public int NumPhoto { get; set; }

        [Column("presentation")]
        [StringLength(1024)]
        public string? Presentation { get; set; }

        [Column("nom")]
        [StringLength(1024)]
        public string Nom { get; set; } = null!;

        [Column("description")]
        [StringLength(1024)]
        public string Description { get; set; } = null!;

        [Column("estbar")]
        public bool? EstBar { get; set; }

        // Navigation FK
        [ForeignKey(nameof(NumPhoto))]
        [InverseProperty(nameof(EntityFramework.Photo.LieuxRestauration))]
        public virtual Photo Photo { get; set; } = null!;

        // Many-to-Many : LieuRestauration <-> Club via fusionne
        public virtual ICollection<Club> Clubs { get; set; } = new List<Club>();
    }
}
