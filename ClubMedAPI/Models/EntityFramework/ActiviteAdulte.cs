using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("activiteadulte")]
    public class ActiviteAdulte
    {
        [Key]
        [Column("idactivite")]
        public int IdActivite { get; set; }

        [Column("numtypeactivite")]
        public int NumTypeActivite { get; set; }

        [Column("titre")]
        [StringLength(1024)]
        public string? Titre { get; set; }

        [Column("description")]
        [StringLength(1024)]
        public string Description { get; set; } = null!;

        [Column("prixmin", TypeName = "numeric")]
        public decimal PrixMin { get; set; }

        [Column("duree", TypeName = "numeric")]
        public decimal Duree { get; set; }

        [Column("ageminimum")]
        public int AgeMinimum { get; set; }

        [Column("frequence")]
        [StringLength(1024)]
        public string Frequence { get; set; } = null!;

        // Navigation FK vers Activite (relation 1-to-1)
        [ForeignKey(nameof(IdActivite))]
        [InverseProperty(nameof(Activite.Adulte))]
        public virtual Activite Activite { get; set; } = null!;

        // Navigation FK vers TypeActivite
        [ForeignKey(nameof(NumTypeActivite))]
        [InverseProperty(nameof(TypeActivite.ActivitesAdultes))]
        public virtual TypeActivite TypeActivite { get; set; } = null!;
    }
}
