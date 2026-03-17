using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("pointfort")]
    public class PointFort
    {
        [Key]
        [Column("numpointfort")]
        public int NumPointFort { get; set; }

        [Column("nom")]
        [StringLength(1024)]
        public string? Nom { get; set; }

        // Many-to-Many : PointFort <-> TypeChambre via synchronise
        public virtual ICollection<TypeChambre> TypeChambres { get; set; } = new List<TypeChambre>();

        // Navigation inverse : Icon
        [InverseProperty(nameof(Icon.PointFort))]
        public virtual ICollection<Icon> Icons { get; set; } = new List<Icon>();
    }
}
