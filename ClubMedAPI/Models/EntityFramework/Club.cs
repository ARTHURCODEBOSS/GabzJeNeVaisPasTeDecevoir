using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("club")]
    public class Club
    {
        [Key]
        [Column("idclub")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdClub { get; set; }

        [Column("numphoto")]
        public int NumPhoto { get; set; }

        [Column("titre")]
        [StringLength(1024)]
        public string? Titre { get; set; }

        [Column("description")]
        [StringLength(1024)]
        public string? Description { get; set; }

        [Column("notemoyenne", TypeName = "numeric")]
        public decimal? NoteMoyenne { get; set; }

        [Column("lienpdf")]
        [StringLength(1024)]
        public string? LienPdf { get; set; }

        [Column("numpays")]
        public int? NumPays { get; set; }

        [Column("email")]
        [StringLength(255)]
        public string? Email { get; set; }

        [Column("statut_mise_en_ligne")]
        [StringLength(50)]
        public string StatutMiseEnLigne { get; set; } = "EN_CREATION";

        // Navigations FK
        [ForeignKey(nameof(NumPays))]
        [InverseProperty(nameof(SousLocalisation.Clubs))]
        public virtual SousLocalisation? Pays { get; set; }

        [ForeignKey(nameof(NumPhoto))]
        [InverseProperty(nameof(EntityFramework.Photo.ClubsAvecCettePhoto))]
        public virtual Photo? Photo { get; set; }

        // Many-to-Many
        public virtual ICollection<Categorie> Categories { get; set; } = new List<Categorie>();
        public virtual ICollection<Regroupement> Regroupements { get; set; } = new List<Regroupement>();
        public virtual ICollection<Activite> Activites { get; set; } = new List<Activite>();
        public virtual ICollection<Chambre> Chambres { get; set; } = new List<Chambre>();
        public virtual ICollection<LieuRestauration> LieuxRestauration { get; set; } = new List<LieuRestauration>();
        public virtual ICollection<Photo> PhotosGalerie { get; set; } = new List<Photo>();
        public virtual ICollection<Station> Stations { get; set; } = new List<Station>();

        // Navigation inverse 1-to-Many
        [InverseProperty(nameof(TypeChambre.Club))]
        public virtual ICollection<TypeChambre> TypeChambres { get; set; } = new List<TypeChambre>();

        [InverseProperty("Club")]
        public virtual ICollection<Avis> Avis { get; set; } = new List<Avis>();

        [InverseProperty(nameof(Reservation.Club))]
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        // Navigation inverse : Disponibilites
        [InverseProperty(nameof(Disponibilite.Club))]
        public virtual ICollection<Disponibilite> Disponibilites { get; set; } = new List<Disponibilite>();
    }
}
