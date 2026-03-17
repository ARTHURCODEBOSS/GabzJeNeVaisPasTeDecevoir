using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("regroupement")]
    public class Regroupement
    {
        [Key]
        [Column("numregroupement")]
        public int NumRegroupement { get; set; }

        [Column("libelleregroupement")]
        [StringLength(1024)]
        public string? LibelleRegroupement { get; set; }

        // Many-to-Many : Regroupement <-> Club via converge_vers
        public virtual ICollection<Club> Clubs { get; set; } = new List<Club>();
    }
}
