using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("date_calendrier")]
    public class DateCalendrier
    {
        [Key]
        [Column("jour")]
        public DateOnly Jour { get; set; }
    }
}
