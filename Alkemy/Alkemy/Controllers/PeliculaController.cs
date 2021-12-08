using Alkemy.Models;
using Alkemy.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using Alkemy.Context;

namespace Alkemy.Controllers
{
   
    [ApiController]
    public class PeliculaController : ControllerBase
    {

        private readonly DisneyContext context;
        public PeliculaController(DisneyContext context)
        {
            this.context = context;
        }


        //-------------------------------------------MOSTRAR PELICULAS-----------------------------------------------  FUNCIONA
        
        [HttpGet("/movies")]
        
        public ActionResult<ApiResponse> Get()
        {
            var response = new ApiResponse();

            try
            {
                foreach (Pelicula p in context.Peliculas.ToList())
                {
                    var peli = context.Peliculas.FirstOrDefault(x => x.IdPelicula.Equals(p.IdPelicula));
                    context.Entry(peli).Reference(x => x.genero).Load();
                }

                response.Ok = true;
                response.Return = context.Peliculas.ToList();
                return response;
            }
            catch (System.Exception ex)
            {
                response.Ok = false;
                response.CodigoError = 1;
                response.Error = "no se encontraron peliculas " + ex.Message;

                return response;
            }

        }

        //-------------------------------------------CREAR PELICULA----------------------------------------------- FUNCIONA

        
        [HttpPost("movies/addMovie")]
        
        public ActionResult<ApiResponse> Post([FromBody] Pelicula p)
        {
            var resultado = new ApiResponse();

            var peli = new Pelicula();
            peli.Titulo = p.Titulo;
            peli.fechaCreacion = p.fechaCreacion;
            peli.Calificacion = p.Calificacion;
            peli.IdGenero = p.IdGenero;

            context.Peliculas.Add(peli);
            context.SaveChanges();

            resultado.Ok = true;
            resultado.Return = context.Peliculas.ToList();
            context.Entry(peli).Reference(x => x.genero).Load();

            return resultado;
        }

        //-------------------------------------------MODIFICAR PELICULA-----------------------------------------------   FUNCIONA

        [HttpPut("/movies/updateMovie")]

        public ActionResult<ApiResponse> Update([FromBody] Pelicula p)
        {
            var resultado = new ApiResponse();

            var peli = context.Peliculas.Where(c => c.IdPelicula == p.IdPelicula).FirstOrDefault();
            if (peli != null)
            {
                peli.Titulo = p.Titulo;
                peli.fechaCreacion = p.fechaCreacion;
                peli.IdGenero = p.IdGenero;

                context.Peliculas.Update(peli);
                context.SaveChanges();
            }

            resultado.Ok = true;
            resultado.Return = context.Peliculas.ToList();
            context.Entry(peli).Reference(x => x.genero).Load();

            return resultado;
        }

        //-------------------------------------------ELIMINAR PELICULA----------------------------------------------- FUNCIONAAA


        [HttpDelete("/movies/deleteMovie/{id}")]

        public ActionResult<ApiResponse> Delete(int id)
        {
            var resultado = new ApiResponse();
            try
            {
                var peli = context.Peliculas.Where(c => c.IdPelicula == id).FirstOrDefault();
                context.Peliculas.Remove(peli);
                context.SaveChanges();

                resultado.Ok = true;
                resultado.Return = context.Peliculas.ToList();         

                return resultado;
            }
            catch (System.Exception ex)
            {
                resultado.Ok = false;
                resultado.Error = "Error no controlado" + ex.Message;

                return resultado;
            }
        }

    }
}
