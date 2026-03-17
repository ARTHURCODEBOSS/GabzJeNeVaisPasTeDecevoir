using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("equipementsalledebain")]
    public class EquipementSalleDeBain
    {
        [Key]
        [Column("numequipementsallebain")]
        public int NumEquipementSalleBain { get; set; }

        [Column("nom")]
        [StringLength(1024)]
        public string? Nom { get; set; }

        // Many-to-Many : EquipementSalleDeBain <-> TypeChambre via assure_la_liaison_avec
        public virtual ICollection<TypeChambre> TypeChambres { get; set; } = new List<TypeChambre>();

        // Navigation inverse : Icon
        [InverseProperty(nameof(Icon.EquipementSalleDeBain))]
        public virtual ICollection<Icon> Icons { get; set; } = new List<Icon>();
    }
}
