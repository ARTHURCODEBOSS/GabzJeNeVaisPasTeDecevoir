using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("typechambre")]
    public class TypeChambre
    {
        [Key]
        [Column("idtypechambre")]
        public int IdTypeChambre { get; set; }

        [Column("numphoto")]
        public int NumPhoto { get; set; }

        [Column("nomtype")]
        [StringLength(1024)]
        public string? NomType { get; set; }

        [Column("metrescarres", TypeName = "numeric")]
        public decimal? MetresCarres { get; set; }

        [Column("textepresentation")]
        [StringLength(1024)]
        public string? TextePresentation { get; set; }

        [Column("capacitemax")]
        public int? CapaciteMax { get; set; }

        [Column("idclub")]
        public int? IdClub { get; set; }

        [Column("indisponible")]
        public bool Indisponible { get; set; } = false;

        // Navigation FK
        [ForeignKey(nameof(IdClub))]
        [InverseProperty(nameof(Club.TypeChambres))]
        public virtual Club? Club { get; set; }

        [ForeignKey(nameof(NumPhoto))]
        [InverseProperty(nameof(EntityFramework.Photo.TypeChambres))]
        public virtual Photo Photo { get; set; } = null!;

        // Navigation inverse
        [InverseProperty(nameof(PrixPeriode.TypeChambre))]
        public virtual ICollection<PrixPeriode> PrixPeriodes { get; set; } = new List<PrixPeriode>();

        [InverseProperty(nameof(Chambre.TypeChambre))]
        public virtual ICollection<Chambre> Chambres { get; set; } = new List<Chambre>();

        // Many-to-Many : TypeChambre <-> Chambre via s_influence_mutuellement
        public virtual ICollection<Chambre> ChambresInfluence { get; set; } = new List<Chambre>();

        // Many-to-Many : TypeChambre <-> PointFort via synchronise
        public virtual ICollection<PointFort> PointForts { get; set; } = new List<PointFort>();

        // Many-to-Many : TypeChambre <-> Service via s_imbrique_dans
        public virtual ICollection<Service> Services { get; set; } = new List<Service>();

        // Many-to-Many : TypeChambre <-> Equipement via se_met_en_harmonie_avec
        public virtual ICollection<Equipement> Equipements { get; set; } = new List<Equipement>();

        // Many-to-Many : TypeChambre <-> EquipementSalleDeBain via assure_la_liaison_avec
        public virtual ICollection<EquipementSalleDeBain> EquipementsSalleDeBain { get; set; } = new List<EquipementSalleDeBain>();
    }
}
