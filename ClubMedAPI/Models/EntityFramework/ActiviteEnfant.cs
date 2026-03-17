using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("activiteenfant")]
    public class ActiviteEnfant
    {
        [Key]
        [Column("idactivite")]
        public int IdActivite { get; set; }

        [Column("numtranche")]
        public int NumTranche { get; set; }

        [Column("titre")]
        [StringLength(1024)]
        public string Titre { get; set; } = null!;

        [Column("description")]
        [StringLength(1024)]
        public string Description { get; set; } = null!;

        [Column("prixmin", TypeName = "numeric")]
        public decimal PrixMin { get; set; }

        [Column("detail")]
        [StringLength(1024)]
        public string Detail { get; set; } = null!;

        // Navigation FK vers Activite (relation 1-to-1)
        [ForeignKey(nameof(IdActivite))]
        [InverseProperty(nameof(Activite.Enfant))]
        public virtual Activite Activite { get; set; } = null!;

        // Navigation FK vers TrancheAge
        [ForeignKey(nameof(NumTranche))]
        [InverseProperty(nameof(TrancheAge.ActivitesEnfants))]
        public virtual TrancheAge TrancheAge { get; set; } = null!;
    }
}
