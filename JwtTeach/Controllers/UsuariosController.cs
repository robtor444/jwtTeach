using JwtTeach.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtTeach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {

        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;

        public UsuariosController(
            //servicio que me permite registrar un usuario
            UserManager<IdentityUser> userManager
            , IConfiguration configuration,
            //sirve para hacer logion al usuario
            SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;

        }


        [HttpPost("registrar")]
        public async Task<ActionResult<RespuestaToken>> Registrar(RegistroUsuario registro)
        {
            //creamos el usuario con email y usuario que es lo que necesita para autenticarse o registrarse 
            var usuario = new IdentityUser { UserName = registro.Nombre, Email = registro.Email };

            //create asyn crear un usuario
            //pasamos el usuario y password
            var resultado = await userManager.CreateAsync(usuario, registro.Password);
            // jsjshjkasfhjhsj
            //si es exitosos
            if (resultado.Succeeded)
            {
                //..aki retornamos el json web token

                //return ConstruirToken(credencialesUsuario);

                return await ConstruirToken(registro,null);
            }
            else
            {
                // sino se puso crear el usuario
                return BadRequest(resultado.Errors);
            }

        }


        private async Task<ActionResult<RespuestaToken>> ConstruirToken(RegistroUsuario registro,Credenciales credenciales)
        {
            //cleim es una informacion acerca del usuario en la cual podemos confiar 
            //el usuario puede leer el claim por eso no se puede poner tarjetas de credito passwords 
            //en el claims
            var claims = new List<Claim>() { };
           dynamic usuario=null ;
            if (credenciales==null)
            {
                claims.Add(new Claim("email", registro.Email));
                claims.Add(new Claim("nombre", registro.Nombre));

                claims.Add(new Claim("everyware", "valo que deseeee no importa"));
               usuario = await userManager.FindByEmailAsync(registro.Email);
            }

            if (registro == null)
            {
                claims.Add(new Claim("nombre", credenciales.Nombre));
                
                claims.Add(new Claim("everyware", "valo que deseeee no importa"));
               usuario = await userManager.FindByNameAsync(credenciales.Nombre);
            }



            //LOS CLAIMS administrador o vendedor ==============================================
            

            //trae todo los claims
            var claimsDb = await userManager.GetClaimsAsync(usuario);

            //fucionamos los claim+ claimsDb
            claims.AddRange(claimsDb);

            //==========================================================


            //contruimos el jwt
            //primero vamos al appsettings a cread una llavejwt
            //Symetric... Representa la clase base abstracta para todas las claves que se generan mediante algoritmos simétricos.
            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llave"]));

            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddYears(1);

            //cstruimos toke
            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiracion, signingCredentials: creds);

            return new RespuestaToken()
            {
                
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken)
                
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<RespuestaToken>> Loguear(Credenciales credenciales)
        {


            //pasamos el usuario y password para el login
            var resultado = await signInManager.PasswordSignInAsync(credenciales.Nombre
                // isPersistent:es para cokkie si usamos,lockoutOnFailure: en caso de los intentos de logueo se bloquee al usuario 
                , credenciales.Password, isPersistent: false, lockoutOnFailure: false);



            //si es exitosos
            if (resultado.Succeeded)
            {
                //..aki retornamos el json web token
                //return ConstruirToken(credencialesUsuario);
                return await ConstruirToken(null,credenciales);
            }
            else
            {
                return BadRequest("Login Incorrecto");
            }

        }

        [HttpGet]
        public async Task<ActionResult<List<RegistroUsuario>>> User()
        {
            var usuarios = userManager.Users;
            return Ok(usuarios);
        }

            [HttpDelete("{name}")]
        public async Task<ActionResult> DeleteUser(string name)
        {
            var user= await userManager.FindByNameAsync(name);

            if (user == null)
                return NotFound("Usuario no creado ni encontrado");
            else
            {
               var resultado= await userManager.DeleteAsync(user);

                if (resultado.Succeeded)
                    return Ok("Usuario borrado con exito");

                else
                {
                    return BadRequest("Algo ocurrio");
                }
                
            }
        }



    }
}
