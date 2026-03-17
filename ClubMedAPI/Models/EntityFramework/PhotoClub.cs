using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    /// <summary>
    /// Table de liaison Club <-> Photo (galerie) via photo_club
    /// Clé composite (idclub + numphoto) définie dans le DbContext
    /// Contient la donnée pivot "ordre"
    /// </summary>
    [Table("photo_club")]
    public class PhotoClub
    {
        [Column("idclub")]
        public int IdClub { get; set; }

        [Column("numphoto")]
        public int NumPhoto { get; set; }

        [Column("ordre")]
        public int? Ordre { get; set; }

        // Navigations FK
        [ForeignKey(nameof(IdClub))]
        public virtual Club Club { get; set; } = null!;

        [ForeignKey(nameof(NumPhoto))]
        public virtual Photo Photo { get; set; } = null!;
    }
}