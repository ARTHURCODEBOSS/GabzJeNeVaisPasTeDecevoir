using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("reservation")]
    public class Reservation
    {
        [Key]
        [Column("numreservation")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NumReservation { get; set; }

        [Column("idclub")]
        public int IdClub { get; set; }

        [Column("idtransport", TypeName = "character(10)")]
        [StringLength(10)]
        public string IdTransport { get; set; } = null!;

        [Column("numclient")]
        public int NumClient { get; set; }

        [Column("datedebut")]
        public DateOnly? DateDebut { get; set; }

        [Column("datefin")]
        public DateOnly? DateFin { get; set; }

        [Column("nbpersonnes")]
        public int? NbPersonnes { get; set; }

        [Column("lieudepart")]
        [StringLength(1024)]
        public string? LieuDepart { get; set; }

        [Column("prix", TypeName = "numeric")]
        public decimal? Prix { get; set; }

        [Column("statut")]
        [StringLength(50)]
        public string? Statut { get; set; }

        [Column("etat_calcule")]
        [StringLength(20)]
        public string? EtatCalcule { get; set; }

        [Column("mail")]
        public bool Mail { get; set; } = false;

        [Column("disponibilite_confirmee")]
        public bool DisponibiliteConfirmee { get; set; } = false;

        [Column("token_partenaire")]
        [StringLength(64)]
        public string? TokenPartenaire { get; set; }

        [Column("mail_confirmation_envoye")]
        public bool? MailConfirmationEnvoye { get; set; }

        [Column("veut_annuler")]
        public bool VeutAnnuler { get; set; } = false;

        // Navigations FK
        [ForeignKey(nameof(IdClub))]
        [InverseProperty(nameof(Club.Reservations))]
        public virtual Club Club { get; set; } = null!;

        [ForeignKey(nameof(IdTransport))]
        [InverseProperty(nameof(Transport.Reservations))]
        public virtual Transport Transport { get; set; } = null!;

        [ForeignKey(nameof(NumClient))]
        [InverseProperty(nameof(Client.Reservations))]
        public virtual Client Client { get; set; } = null!;

        // Navigation inverse
        [InverseProperty(nameof(AutreVoyageur.Reservation))]
        public virtual ICollection<AutreVoyageur> AutresVoyageurs { get; set; } = new List<AutreVoyageur>();

        [InverseProperty("Reservation")]
        public virtual ICollection<Avis> Avis { get; set; } = new List<Avis>();

        [InverseProperty(nameof(Transaction.Reservation))]
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        // Many-to-Many : Reservation <-> Activite via se_lie_a
        public virtual ICollection<ReservationActivite> ReservationActivites { get; set; } = new List<ReservationActivite>();
    }
}
