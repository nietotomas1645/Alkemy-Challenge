using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alkemy.Models
{
    public class Pelicula
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int? IdPelicula { get; set; }

        public string Titulo { get; set; }

        public DateTime fechaCreacion { get; set; }

        public int Calificacion { get; set; }

        //===================================
        public int IdGenero { get; set; }
        [ForeignKey("IdGenero")]

        public Genero genero { get; set; }


        public Pelicula()
        {

        }

    }
}
