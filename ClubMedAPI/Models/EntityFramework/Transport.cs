using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("transport")]
    public class Transport
    {
        [Key]
        [Column("idtransport", TypeName = "character(10)")]
        [StringLength(10)]
        public string IdTransport { get; set; } = null!;

        [Column("lieudepart")]
        [StringLength(1024)]
        public string? LieuDepart { get; set; }

        [Column("prix", TypeName = "numeric(10,2)")]
        public decimal? Prix { get; set; }

        // Navigation inverse
        [InverseProperty(nameof(Reservation.Transport))]
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
