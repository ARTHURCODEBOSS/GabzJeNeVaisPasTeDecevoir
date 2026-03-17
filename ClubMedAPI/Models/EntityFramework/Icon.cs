using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("icon")]
    public class Icon
    {
        [Key]
        [Column("numicon")]
        public int NumIcon { get; set; }

        [Column("numpointfort")]
        public int NumPointFort { get; set; }

        [Column("numservice")]
        public int NumService { get; set; }

        [Column("numequipementsallebain")]
        public int NumEquipementSalleBain { get; set; }

        [Column("lienicon")]
        [StringLength(1024)]
        public string? LienIcon { get; set; }

        // Navigations FK
        [ForeignKey(nameof(NumPointFort))]
        [InverseProperty(nameof(EntityFramework.PointFort.Icons))]
        public virtual PointFort PointFort { get; set; } = null!;

        [ForeignKey(nameof(NumService))]
        [InverseProperty(nameof(EntityFramework.Service.Icons))]
        public virtual Service Service { get; set; } = null!;

        [ForeignKey(nameof(NumEquipementSalleBain))]
        [InverseProperty(nameof(EntityFramework.EquipementSalleDeBain.Icons))]
        public virtual EquipementSalleDeBain EquipementSalleDeBain { get; set; } = null!;

        // Navigation inverse : Equipements qui utilisent cette icône
        [InverseProperty(nameof(Equipement.Icon))]
        public virtual ICollection<Equipement> Equipements { get; set; } = new List<Equipement>();
    }
}
