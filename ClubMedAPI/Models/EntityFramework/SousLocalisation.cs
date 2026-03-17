using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("souslocalisation")]
    public class SousLocalisation
    {
        [Key]
        [Column("numpays")]
        public int NumPays { get; set; }

        [Column("numphoto")]
        public int NumPhoto { get; set; }

        [Column("nompays")]
        [StringLength(1024)]
        public string? NomPays { get; set; }

        // Navigation FK vers Photo
        [ForeignKey(nameof(NumPhoto))]
        [InverseProperty(nameof(EntityFramework.Photo.SousLocalisations))]
        public virtual Photo Photo { get; set; } = null!;

        // Many-to-Many : SousLocalisation <-> Localisation via s_articule_autour_de
        public virtual ICollection<Localisation> Localisations { get; set; } = new List<Localisation>();

        // Navigation inverse : les clubs de ce pays
        [InverseProperty(nameof(Club.Pays))]
        public virtual ICollection<Club> Clubs { get; set; } = new List<Club>();
    }
}
