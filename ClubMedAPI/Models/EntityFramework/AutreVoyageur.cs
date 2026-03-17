using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("autrevoyageur")]
    public class AutreVoyageur
    {
        [Key]
        [Column("numvoyageur")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NumVoyageur { get; set; }

        [Column("numreservation")]
        public int NumReservation { get; set; }

        [Column("genre")]
        [StringLength(1024)]
        public string? Genre { get; set; }

        [Column("prenom")]
        [StringLength(1024)]
        public string? Prenom { get; set; }

        [Column("nom")]
        [StringLength(1024)]
        public string? Nom { get; set; }

        [Column("datenaissance")]
        public DateOnly? DateNaissance { get; set; }

        // Navigation FK
        [ForeignKey(nameof(NumReservation))]
        [InverseProperty(nameof(Reservation.AutresVoyageurs))]
        public virtual Reservation Reservation { get; set; } = null!;
    }
}
