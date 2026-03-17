using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("carte_bancaire")]
    public class CarteBancaire
    {
        [Key]
        [Column("idcb")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCb { get; set; }

        [Column("numclient")]
        public int? NumClient { get; set; }

        [Column("numcartebancaire_crypter")]
        [StringLength(1024)]
        public string? NumCarteBancaireCrypter { get; set; }

        [Column("dateexpiration_carte_bancaire")]
        [StringLength(5)]
        public string? DateExpirationCarteBancaire { get; set; }

        [Column("cvv_crypter")]
        [StringLength(1024)]
        public string? CvvCrypter { get; set; }

        [Column("est_active")]
        public bool EstActive { get; set; } = true;

        // Navigation
        [ForeignKey(nameof(NumClient))]
        [InverseProperty(nameof(Client.CartesBancaires))]
        public virtual Client? Client { get; set; }

        // Navigation inverse
        [InverseProperty(nameof(Transaction.CarteBancaire))]
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
