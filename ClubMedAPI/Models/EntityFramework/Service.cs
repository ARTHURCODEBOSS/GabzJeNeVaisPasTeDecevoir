using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("service")]
    public class Service
    {
        [Key]
        [Column("numservice")]
        public int NumService { get; set; }

        [Column("nom")]
        [StringLength(1024)]
        public string? Nom { get; set; }

        // Many-to-Many : Service <-> TypeChambre via s_imbrique_dans
        public virtual ICollection<TypeChambre> TypeChambres { get; set; } = new List<TypeChambre>();

        // Navigation inverse : Icon
        [InverseProperty(nameof(Icon.Service))]
        public virtual ICollection<Icon> Icons { get; set; } = new List<Icon>();
    }
}
