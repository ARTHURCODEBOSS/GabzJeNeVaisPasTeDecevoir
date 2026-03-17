using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("localisation")]
    public class Localisation
    {
        [Key]
        [Column("numlocalisation")]
        public int NumLocalisation { get; set; }

        [Column("nomlocalisation")]
        [StringLength(1024)]
        public string? NomLocalisation { get; set; }

        // Many-to-Many : Localisation <-> SousLocalisation (pays) via s_articule_autour_de
        public virtual ICollection<SousLocalisation> SousLocalisations { get; set; } = new List<SousLocalisation>();

        // Many-to-Many : Localisation <-> Categorie via fusionne_avec
        public virtual ICollection<Categorie> Categories { get; set; } = new List<Categorie>();
    }
}
