using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    /// <summary>
    /// Table de liaison Club <-> Station via clubstation
    /// Clé composite (idclub + numstation) définie dans le DbContext
    /// Contient des données pivot (altitudeclub, titre, description, etc.)
    /// </summary>
    [Table("clubstation")]
    public class ClubStation
    {
        [Column("idclub")]
        public int IdClub { get; set; }

        [Column("numstation")]
        public int NumStation { get; set; }

        [Column("numphoto")]
        public int? NumPhoto { get; set; }

        [Column("titre")]
        [StringLength(1024)]
        public string? Titre { get; set; }

        [Column("description")]
        [StringLength(1024)]
        public string Description { get; set; } = null!;

        [Column("notemoyenne", TypeName = "numeric")]
        public decimal? NoteMoyenne { get; set; }

        [Column("lienpdf")]
        [StringLength(1024)]
        public string? LienPdf { get; set; }

        [Column("altitudeclub", TypeName = "character(10)")]
        [StringLength(10)]
        public string AltitudeClub { get; set; } = null!;

        // Navigations FK
        [ForeignKey(nameof(IdClub))]
        public virtual Club Club { get; set; } = null!;

        [ForeignKey(nameof(NumStation))]
        public virtual Station Station { get; set; } = null!;

        [ForeignKey(nameof(NumPhoto))]
        public virtual Photo? Photo { get; set; }
    }
}
