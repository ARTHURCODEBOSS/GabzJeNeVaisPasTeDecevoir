using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("chambre")]
    public class Chambre
    {
        [Key]
        [Column("numchambre")]
        public int NumChambre { get; set; }

        [Column("idtypechambre")]
        public int IdTypeChambre { get; set; }

        // Navigation FK
        [ForeignKey(nameof(IdTypeChambre))]
        [InverseProperty(nameof(TypeChambre.Chambres))]
        public virtual TypeChambre TypeChambre { get; set; } = null!;

        // Many-to-Many : Chambre <-> Club via s_unit_a
        public virtual ICollection<Club> Clubs { get; set; } = new List<Club>();

        // Many-to-Many : Chambre <-> TypeChambre via s_influence_mutuellement
        public virtual ICollection<TypeChambre> TypeChambresInfluence { get; set; } = new List<TypeChambre>();

        // Navigation inverse : Disponibilites
        [InverseProperty(nameof(Disponibilite.Chambre))]
        public virtual ICollection<Disponibilite> Disponibilites { get; set; } = new List<Disponibilite>();
    }
}
