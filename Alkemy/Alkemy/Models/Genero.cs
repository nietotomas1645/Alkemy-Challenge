using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alkemy.Models
{
    public class Genero
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int IdGenero { get; set; }

        public string Nombre { get; set; }
    }
}
