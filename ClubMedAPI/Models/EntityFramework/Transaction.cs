using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("transaction")]
    public class Transaction
    {
        [Key]
        [Column("idtransaction")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdTransaction { get; set; }

        [Column("numreservation")]
        public int NumReservation { get; set; }

        [Column("montant", TypeName = "numeric")]
        public decimal? Montant { get; set; }

        [Column("date_transaction", TypeName = "timestamp without time zone")]
        public DateTime? DateTransaction { get; set; }

        [Column("moyen_paiement")]
        [StringLength(50)]
        public string? MoyenPaiement { get; set; }

        [Column("statut_paiement")]
        [StringLength(50)]
        public string? StatutPaiement { get; set; }

        [Column("idcb")]
        public int? IdCb { get; set; }

        // Navigations FK
        [ForeignKey(nameof(NumReservation))]
        [InverseProperty(nameof(Reservation.Transactions))]
        public virtual Reservation Reservation { get; set; } = null!;

        [ForeignKey(nameof(IdCb))]
        [InverseProperty(nameof(CarteBancaire.Transactions))]
        public virtual CarteBancaire? CarteBancaire { get; set; }
    }
}
