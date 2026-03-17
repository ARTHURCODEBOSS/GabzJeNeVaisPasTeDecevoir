using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("categorie")]
    public class Categorie
    {
        [Key]
        [Column("numcategory")]
        public int NumCategory { get; set; }

        [Column("nomcategory")]
        [StringLength(1024)]
        public string? NomCategory { get; set; }

        // Many-to-Many : Categorie <-> Club via collabore
        public virtual ICollection<Club> Clubs { get; set; } = new List<Club>();

        // Many-to-Many : Categorie <-> Localisation via fusionne_avec
        public virtual ICollection<Localisation> Localisations { get; set; } = new List<Localisation>();

        // Many-to-Many : Categorie <-> TypeClub via s_harmonise_avec
        public virtual ICollection<TypeClub> TypeClubs { get; set; } = new List<TypeClub>();
    }
}
