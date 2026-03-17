using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("activite")]
    public class Activite
    {
        [Key]
        [Column("idactivite")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdActivite { get; set; }

        [Column("titre")]
        [StringLength(1024)]
        public string? Titre { get; set; }

        [Column("description")]
        [StringLength(1024)]
        public string Description { get; set; } = null!;

        [Column("prixmin", TypeName = "numeric")]
        public decimal PrixMin { get; set; }

        [Column("idpartenaire")]
        public int? IdPartenaire { get; set; }

        // Navigation FK vers Partenaire
        [ForeignKey(nameof(IdPartenaire))]
        [InverseProperty(nameof(Partenaire.Activites))]
        public virtual Partenaire? Partenaire { get; set; }

        // Héritage : une activité est soit adulte, soit enfant
        [InverseProperty(nameof(ActiviteAdulte.Activite))]
        public virtual ActiviteAdulte? Adulte { get; set; }

        [InverseProperty(nameof(ActiviteEnfant.Activite))]
        public virtual ActiviteEnfant? Enfant { get; set; }

        // Many-to-Many : Activite <-> Club via incruste_avec
        public virtual ICollection<Club> Clubs { get; set; } = new List<Club>();

        // Many-to-Many : Activite <-> Reservation via se_lie_a
        public virtual ICollection<ReservationActivite> ReservationActivites { get; set; } = new List<ReservationActivite>();
    }
}
