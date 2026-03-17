using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("client")]
    public class Client
    {
        [Key]
        [Column("numclient")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NumClient { get; set; }

        [Column("numadresse")]
        public int? NumAdresse { get; set; }

        [Column("genre")]
        [StringLength(1024)]
        public string? Genre { get; set; }

        [Column("prenom")]
        [StringLength(1024)]
        public string Prenom { get; set; } = null!;

        [Column("nom")]
        [StringLength(1024)]
        public string Nom { get; set; } = null!;

        [Column("datenaissance")]
        public DateOnly? DateNaissance { get; set; }

        [Column("email")]
        [StringLength(1024)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Column("telephone")]
        [StringLength(1024)]
        public string? Telephone { get; set; }

        [Column("motdepasse_crypter")]
        [StringLength(1024)]
        public string MotDePasseCrypter { get; set; } = null!;

        [Column("numcartebancaire_crypter")]
        [StringLength(1024)]
        public string? NumCarteBancaireCrypter { get; set; }

        [Column("dateexpiration_carte_bancaire")]
        [StringLength(5)]
        public string? DateExpirationCarteBancaire { get; set; }

        [Column("cvv_crypter")]
        [StringLength(1024)]
        public string? CvvCrypter { get; set; }

        [Column("role")]
        [StringLength(20)]
        public string Role { get; set; } = "client";

        [Column("a2f_active")]
        public bool A2fActive { get; set; } = false;

        [Column("a2f_method")]
        [StringLength(10)]
        public string? A2fMethod { get; set; } // "SMS" ou "TOTP"

        [Column("totp_secret")]
        [StringLength(255)]
        public string? TotpSecret { get; set; }

        [Column("stripe_id")]
        [StringLength(255)]
        public string? StripeId { get; set; }

        [Column("pm_type")]
        [StringLength(255)]
        public string? PmType { get; set; }

        [Column("pm_last_four")]
        [StringLength(4)]
        public string? PmLastFour { get; set; }

        [Column("trial_ends_at", TypeName = "timestamp(0) without time zone")]
        public DateTime? TrialEndsAt { get; set; }

        // Navigation
        [ForeignKey(nameof(NumAdresse))]
        [InverseProperty(nameof(Adresse.Clients))]
        public virtual Adresse? Adresse { get; set; }

        // Navigation inverse
        [InverseProperty(nameof(Reservation.Client))]
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        [InverseProperty("Client")]
        public virtual ICollection<Avis> Avis { get; set; } = new List<Avis>();

        [InverseProperty(nameof(CarteBancaire.Client))]
        public virtual ICollection<CarteBancaire> CartesBancaires { get; set; } = new List<CarteBancaire>();
    }
}
