using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMedAPI.Models.EntityFramework
{
    [Table("adresse")]
    public class Adresse
    {
        [Key]
        [Column("numadresse")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NumAdresse { get; set; }

        [Column("numrue")]
        public int NumRue { get; set; }

        [Column("nomrue")]
        [StringLength(1024)]
        public string NomRue { get; set; } = null!;

        [Column("codepostal")]
        [StringLength(5)]
        public string CodePostal { get; set; } = null!;

        [Column("ville")]
        [StringLength(1024)]
        public string Ville { get; set; } = null!;

        [Column("pays")]
        [StringLength(1024)]
        public string Pays { get; set; } = null!;

        // Navigation inverse
        [InverseProperty(nameof(Client.Adresse))]
        public virtual ICollection<Client> Clients { get; set; } = new List<Client>();
    }
}
