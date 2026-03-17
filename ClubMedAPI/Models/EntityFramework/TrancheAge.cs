using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("trancheage")]
    public class TrancheAge
    {
        [Key]
        [Column("numtranche")]
        public int NumTranche { get; set; }

        [Column("agemin")]
        public int? AgeMin { get; set; }

        [Column("agemax")]
        public int? AgeMax { get; set; }

        // Navigation inverse
        [InverseProperty(nameof(ActiviteEnfant.TrancheAge))]
        public virtual ICollection<ActiviteEnfant> ActivitesEnfants { get; set; } = new List<ActiviteEnfant>();
    }
}
