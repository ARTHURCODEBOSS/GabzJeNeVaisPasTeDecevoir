using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("equipement")]
    public class Equipement
    {
        [Key]
        [Column("numequipement")]
        public int NumEquipement { get; set; }

        [Column("numicon")]
        public int NumIcon { get; set; }

        [Column("nom")]
        [StringLength(1024)]
        public string? Nom { get; set; }

        // Navigation FK
        [ForeignKey(nameof(NumIcon))]
        [InverseProperty(nameof(Icon.Equipements))]
        public virtual Icon Icon { get; set; } = null!;

        // Many-to-Many : Equipement <-> TypeChambre via se_met_en_harmonie_avec
        public virtual ICollection<TypeChambre> TypeChambres { get; set; } = new List<TypeChambre>();
    }
}
