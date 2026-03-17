using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("periode")]
    public class Periode
    {
        [Key]
        [Column("numperiode", TypeName = "character(10)")]
        [StringLength(10)]
        public string NumPeriode { get; set; } = null!;

        [Column("datedeb")]
        public DateOnly? DateDeb { get; set; }

        [Column("datefin")]
        public DateOnly? DateFin { get; set; }

        // Navigation inverse
        [InverseProperty(nameof(PrixPeriode.Periode))]
        public virtual ICollection<PrixPeriode> PrixPeriodes { get; set; } = new List<PrixPeriode>();
    }
}
