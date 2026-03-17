using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("typeclub")]
    public class TypeClub
    {
        [Key]
        [Column("numtype")]
        public int NumType { get; set; }

        [Column("nomtype")]
        [StringLength(1024)]
        public string? NomType { get; set; }

        // Many-to-Many : TypeClub <-> Categorie via s_harmonise_avec
        public virtual ICollection<Categorie> Categories { get; set; } = new List<Categorie>();
    }
}
