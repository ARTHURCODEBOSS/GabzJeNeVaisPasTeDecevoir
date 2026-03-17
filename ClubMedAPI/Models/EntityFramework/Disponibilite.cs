using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    /// <summary>
    /// Table de liaison Calendrier <-> Chambre <-> Club
    /// Clé composite (date + numchambre + idclub) définie dans le DbContext
    /// </summary>
    [Table("disponibilite")]
    public class Disponibilite
    {
        [Column("date")]
        public DateOnly Date { get; set; }

        [Column("numchambre")]
        public int NumChambre { get; set; }

        [Column("idclub")]
        public int IdClub { get; set; }

        [Column("estdisponibilite")]
        public bool? EstDisponibilite { get; set; }

        // Navigations FK
        [ForeignKey(nameof(NumChambre))]
        [InverseProperty(nameof(EntityFramework.Chambre.Disponibilites))]
        public virtual Chambre Chambre { get; set; } = null!;

        [ForeignKey(nameof(IdClub))]
        [InverseProperty(nameof(EntityFramework.Club.Disponibilites))]
        public virtual Club Club { get; set; } = null!;
    }
}
