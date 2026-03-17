using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("avis")]
    public class Avis
    {
        [Key]
        [Column("numavis")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NumAvis { get; set; }

        [Column("idclub")]
        public int IdClub { get; set; }

        [Column("numclient")]
        public int NumClient { get; set; }

        [Column("titre")]
        [StringLength(1024)]
        public string? Titre { get; set; }

        [Column("commentaire")]
        [StringLength(1024)]
        public string Commentaire { get; set; } = null!;

        [Column("note")]
        public int Note { get; set; }

        [Column("numreservation")]
        public int NumReservation { get; set; }

        [Column("reponse")]
        [StringLength(1024)]
        public string? Reponse { get; set; }

        // Navigations FK
        [ForeignKey(nameof(IdClub))]
        [InverseProperty(nameof(Club.Avis))]
        public virtual Club Club { get; set; } = null!;

        [ForeignKey(nameof(NumClient))]
        [InverseProperty(nameof(Client.Avis))]
        public virtual Client Client { get; set; } = null!;

        [ForeignKey(nameof(NumReservation))]
        [InverseProperty(nameof(Reservation.Avis))]
        public virtual Reservation Reservation { get; set; } = null!;

        // Many-to-Many : Avis <-> Photo via photoavis
        public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();
    }
}
