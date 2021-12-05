using Alkemy.Context;
using Alkemy.Models;
using Alkemy.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Alkemy.Controllers
{
    [ApiController]
    public class PersonajeController : ControllerBase
    {
        private readonly DisneyContext context;
        public PersonajeController(DisneyContext context)
        {
            this.context = context;
        }


        //-----------------------------MOSTRAR PERSONAJES---------------------------------------------  FUNCIONA

        [HttpGet]
        [Route("/characters")]
        public ActionResult<ApiResponse> Get()
        {
            var response = new ApiResponse();

            try
            {
                foreach (Personaje p in context.Personajes.ToList())
                {
                    var peli = context.Personajes.FirstOrDefault(x => x.IdPersonaje.Equals(p.IdPersonaje));

                }
                response.Ok = true;
                response.Return = context.Personajes.ToList();

                return response;
            }
            catch (Exception ex)
            {
                response.Ok = false;
                response.Error = "ERROR se requiere autorizacion" + ex.Message;
                return response;
            }

        }

        //-------------------------------- CREAR PERSONAJE------------------------------------ FUNCIONA

        [HttpPost]
        [Route("characters/addCharacter")]
        
        public ActionResult<ApiResponse> Post([FromBody] Personaje p)
        {
            var response = new ApiResponse();

            var personaje = new Personaje();
            personaje.Imagen = p.Imagen;
            personaje.Nombre = p.Nombre;            
            personaje.Edad = p.Edad;
            personaje.Peso = p.Peso; 
            personaje.Historia = p.Historia;   
            personaje.idPelicula = p.idPelicula;

            context.Personajes.Add(personaje);
            context.SaveChanges();

            response.Ok = true;
            response.Return = context.Personajes.ToList();
            context.Entry(personaje).Reference(x => x.pelicula).Load();

            return response;
        }


        // --------------------------------- MODIFICAR PERSONAJE--------------------------- FUNCIONA
        [HttpPut]
        [Route("characters/updateCharacter")]

        public ActionResult<ApiResponse> Put([FromBody] Personaje p)
        {
            var response = new ApiResponse();

            var personaje = context.Personajes.Where(c => c.IdPersonaje == p.IdPersonaje).FirstOrDefault();
            if (personaje != null)
            {
                personaje.Imagen = p.Imagen;
                personaje.Nombre = p.Nombre;
                personaje.Edad = p.Edad;
                personaje.Peso = p.Peso;
                personaje.Historia = p.Historia;
                personaje.idPelicula = p.idPelicula;

                context.Personajes.Update(personaje);
                context.SaveChanges();
            }

            response.Ok = true;
            response.Return = context.Peliculas.ToList();
            context.Entry(personaje).Reference(x => x.pelicula).Load();

            return response;
        }


        // ------------------------------------ELIMINAR PERSONAJE --------------------------------------------  
        [HttpDelete("/characters/deleteCharacter/{id}")]

        public ActionResult<ApiResponse> Delete(int id)
        {
            var response = new ApiResponse();
            try
            {
                var personaje = context.Personajes.Where(c => c.IdPersonaje == id).FirstOrDefault();
                context.Personajes.Remove(personaje);
                context.SaveChanges();

               response.Ok = true;
                response.Return = context.Peliculas.ToList();

                return response;
            }
            catch (System.Exception ex)
            {
                response.Ok = false;
                response.Error = "Error no controlado" + ex.Message;

                return response;
            }
        }
    }
}
