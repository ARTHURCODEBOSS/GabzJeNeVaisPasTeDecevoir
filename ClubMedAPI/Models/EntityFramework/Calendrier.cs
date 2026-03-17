using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("calendrier")]
    public class Calendrier
    {
        [Key]
        [Column("date")]
        public DateOnly Date { get; set; }
    }
}
