using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    /// <summary>
    /// Table de liaison avec clé composite (numperiode + idtypechambre)
    /// La clé composite sera définie dans le DbContext via Fluent API
    /// </summary>
    [Table("prix_periode")]
    public class PrixPeriode
    {
        [Column("numperiode", TypeName = "character(10)")]
        [StringLength(10)]
        public string NumPeriode { get; set; } = null!;

        [Column("idtypechambre")]
        public int IdTypeChambre { get; set; }

        [Column("prixperiode", TypeName = "numeric")]
        public decimal? PrixPeriodeValue { get; set; }

        // Navigation FK
        [ForeignKey(nameof(NumPeriode))]
        [InverseProperty(nameof(Periode.PrixPeriodes))]
        public virtual Periode Periode { get; set; } = null!;

        [ForeignKey(nameof(IdTypeChambre))]
        [InverseProperty(nameof(TypeChambre.PrixPeriodes))]
        public virtual TypeChambre TypeChambre { get; set; } = null!;
    }
}
