using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("partenaires")]
    public class Partenaire
    {
        [Key]
        [Column("idpartenaire")]
        public int IdPartenaire { get; set; }

        [Column("nom")]
        [StringLength(255)]
        public string? Nom { get; set; }

        [Column("email")]
        [StringLength(255)]
        [EmailAddress]
        public string? Email { get; set; }

        [Column("telephone")]
        [StringLength(50)]
        public string? Telephone { get; set; }

        // Navigation inverse
        [InverseProperty(nameof(Activite.Partenaire))]
        public virtual ICollection<Activite> Activites { get; set; } = new List<Activite>();
    }
}
