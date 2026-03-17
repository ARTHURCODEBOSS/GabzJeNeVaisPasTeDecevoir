using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("station")]
    public class Station
    {
        [Key]
        [Column("numstation")]
        public int NumStation { get; set; }

        [Column("numphoto")]
        public int NumPhoto { get; set; }

        [Column("nomstation")]
        [StringLength(1024)]
        public string NomStation { get; set; } = null!;

        [Column("altitudestation", TypeName = "numeric")]
        public decimal AltitudeStation { get; set; }

        [Column("longueurpistes", TypeName = "numeric")]
        public decimal LongueurPistes { get; set; }

        [Column("nbpistes")]
        public int NbPistes { get; set; }

        [Column("infoski")]
        [StringLength(1024)]
        public string InfoSki { get; set; } = null!;

        // Navigation FK vers Photo
        [ForeignKey(nameof(NumPhoto))]
        [InverseProperty(nameof(EntityFramework.Photo.Stations))]
        public virtual Photo Photo { get; set; } = null!;

        // Many-to-Many : Station <-> Club via clubstation
        public virtual ICollection<Club> Clubs { get; set; } = new List<Club>();
    }
}
