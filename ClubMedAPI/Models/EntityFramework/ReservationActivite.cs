using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    /// <summary>
    /// Table de liaison Reservation <-> Activite via se_lie_a
    /// Clé composite (numreservation + idactivite) définie dans le DbContext
    /// </summary>
    [Table("se_lie_a")]
    public class ReservationActivite
    {
        [Column("numreservation")]
        public int NumReservation { get; set; }

        [Column("idactivite")]
        public int IdActivite { get; set; }

        [Column("nbpersonnes")]
        public int NbPersonnes { get; set; }

        [Column("disponibilite_confirmee")]
        public bool DisponibiliteConfirmee { get; set; } = false;

        [Column("token")]
        [StringLength(255)]
        public string? Token { get; set; }

        [Column("date_envoi")]
        public DateOnly? DateEnvoi { get; set; }

        // Navigations FK
        [ForeignKey(nameof(NumReservation))]
        [InverseProperty(nameof(Reservation.ReservationActivites))]
        public virtual Reservation Reservation { get; set; } = null!;

        [ForeignKey(nameof(IdActivite))]
        [InverseProperty(nameof(Activite.ReservationActivites))]
        public virtual Activite Activite { get; set; } = null!;
    }
}
