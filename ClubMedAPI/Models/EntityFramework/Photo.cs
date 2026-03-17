using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("photo")]
    public class Photo
    {
        [Key]
        [Column("numphoto")]
        public int NumPhoto { get; set; }

        [Column("url")]
        [StringLength(1024)]
        public string? Url { get; set; }

        // Navigation inverse
        [InverseProperty(nameof(Club.Photo))]
        public virtual ICollection<Club> ClubsAvecCettePhoto { get; set; } = new List<Club>();

        [InverseProperty(nameof(TypeChambre.Photo))]
        public virtual ICollection<TypeChambre> TypeChambres { get; set; } = new List<TypeChambre>();

        [InverseProperty(nameof(TypeActivite.Photo))]
        public virtual ICollection<TypeActivite> TypeActivites { get; set; } = new List<TypeActivite>();

        [InverseProperty(nameof(LieuRestauration.Photo))]
        public virtual ICollection<LieuRestauration> LieuxRestauration { get; set; } = new List<LieuRestauration>();

        [InverseProperty(nameof(SousLocalisation.Photo))]
        public virtual ICollection<SousLocalisation> SousLocalisations { get; set; } = new List<SousLocalisation>();

        [InverseProperty(nameof(Station.Photo))]
        public virtual ICollection<Station> Stations { get; set; } = new List<Station>();

        // Many-to-Many : Photo <-> Club (galerie)
        [InverseProperty(nameof(Club.PhotosGalerie))]
        public virtual ICollection<Club> ClubsGalerie { get; set; } = new List<Club>();

        // Many-to-Many : Photo <-> Avis
        [InverseProperty("Photos")]
        public virtual ICollection<Avis> Avis { get; set; } = new List<Avis>();
    }
}
