using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("typeactivite")]
    public class TypeActivite
    {
        [Key]
        [Column("numtypeactivite")]
        public int NumTypeActivite { get; set; }

        [Column("numphoto")]
        public int NumPhoto { get; set; }

        [Column("description")]
        [StringLength(4096)]
        public string Description { get; set; } = null!;

        [Column("nbactivitecarte")]
        public int NbActiviteCarte { get; set; }

        [Column("nbactiviteincluse")]
        public int NbActiviteIncluse { get; set; }

        [Column("titre")]
        [StringLength(1024)]
        public string? Titre { get; set; }

        // Navigation FK
        [ForeignKey(nameof(NumPhoto))]
        [InverseProperty(nameof(EntityFramework.Photo.TypeActivites))]
        public virtual Photo Photo { get; set; } = null!;

        // Navigation inverse
        [InverseProperty(nameof(ActiviteAdulte.TypeActivite))]
        public virtual ICollection<ActiviteAdulte> ActivitesAdultes { get; set; } = new List<ActiviteAdulte>();
    }
}
