using Alkemy.Context;
using Alkemy.Models;
using Alkemy.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;



namespace Alkemy.Controllers
{

    [ApiController]
    
    public class UsuarioController : ControllerBase
    {
        private readonly DisneyContext context;
        private readonly IConfiguration _configuration;
       
        public UsuarioController(DisneyContext context, IConfiguration configuration)
        {
            this.context = context;
            _configuration = configuration;

        }

        // ---------------------------- LEER USUARIOS----------------------------------------------
        [HttpGet]
        [Route("/get/users")]
        

        public ActionResult<ApiResponse> Get()
        {
            var response = new ApiResponse();

            try
            {
                foreach (Usuario usu in context.Usuarios.ToList())
                {
                    var user = context.Usuarios.FirstOrDefault(x => x.IdUsuario.Equals(usu.IdUsuario));
                    
                }

                response.Ok = true;
                response.Return = context.Usuarios.ToList();

                return response;
            }
            catch (Exception ex)
            {
                response.Ok = false;
                response.CodigoError = 404;
                response.Error = "Error al encontrar usuarios " + ex;

                return response;
            }
        }

        // ------------------------- inicio sesion-----------------------------------------

        [HttpPost]
        [Route("/auth/login")]
        [AllowAnonymous]
        public ActionResult<ApiResponse> Login([FromBody] Usuario u)
        {
            var response = new ApiResponse();
            var email = u.Email.Trim();
            var password = u.Password;
            try
            {
                var usu = context.Usuarios.FirstOrDefault(x => x.Email.Equals(email) && x.Password.Equals(password));
                if (usu != null)
                {
                    // Leemos el secret_key desde nuestro appseting
                    var secretKey = _configuration.GetValue<string>("SecretKey");
                    var key = Encoding.ASCII.GetBytes(secretKey);

                    // Creamos los claims (pertenencias, características) del usuario
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                     {
                        new Claim(ClaimTypes.NameIdentifier, usu.IdUsuario.ToString()),
                        new Claim(ClaimTypes.Email, usu.Email)
                     }),
                        // Nuestro token va a durar una hora
                        Expires = DateTime.UtcNow.AddHours(1),
                        // Credenciales para generar el token usando nuestro secretykey y el algoritmo hash 256
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var createdToken = tokenHandler.CreateToken(tokenDescriptor);



                    response.Ok = true;
                    response.Return = tokenHandler.WriteToken(createdToken);
                }
                else
                {
                    response.Ok = false;
                    response.Error = "Usuario o contraseña incorrectos!";
                }

                return response;

            }
            catch (Exception ex)
            {
                response.Ok = false;
                response.CodigoError = 3;
                response.Error = "Error al conectarse, intente nuevamente! " + ex.Message;

                return response;
            }
        }
        // --------------------------------------- REGISTRO USUARIO -------------------------------------

        [HttpPost]
        [Route("/auth/register")]
        [AllowAnonymous]
        public ActionResult<ApiResponse> Register([FromBody] Usuario u)
        {
            var response = new ApiResponse();

            var user = new Usuario();
            user.Email = u.Email;
            user.Password = u.Password;


            context.Usuarios.Add(user);
            context.SaveChanges();

            response.Ok = true;
            response.Return = "Registrado con éxito! Email: " + user.Email + " Password: " + user.Password;

            return response;

        }

    }
}
