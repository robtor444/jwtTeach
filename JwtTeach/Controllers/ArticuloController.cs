
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JwtTeach.Modelo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using JwtTeach.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace JwtTeach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticuloController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public ArticuloController( ApplicationDbContext context, IMapper mapper 
            //permite conseguir el id del usuario a partir de su email
            ,UserManager<IdentityUser> userManager
            )
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<Articulo>>> GetArticulos()
        {
            var articulos = await context.Articulos
                .Include(comentario=> comentario.Usuario)
                .ToListAsync();

            return Ok(articulos);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResult> PostArticulo(CreaArticuloDTO articuloDtoNuevo)
        {
            //acceder al claim nombre por medio del Http
            var nombreClaim = HttpContext.User.Claims.Where(claim => claim.Type == "nombre")
                .FirstOrDefault();
            //saco del email claim .value el email o nombre por que trae varios campos
            var nombre = nombreClaim.Value;

            //hacemos la busqueda
            var usuario =await userManager.FindByNameAsync(nombre);

            // del usuario anterios solo quiero el id
            var usuarioId = usuario.Id;



            var articuloNuevo = mapper.Map<Articulo>(articuloDtoNuevo);

            //aki le pongo el id de quien se logueo
            articuloNuevo.UsuarioId=usuarioId;

           await context.Articulos.AddAsync(articuloNuevo);
           await  context.SaveChangesAsync();

            return Ok(articuloDtoNuevo);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Articulo>> PutArticulo(Articulo articuloDtoNuevo, int id)
        {

            var existe = await context.Articulos.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound("El id de autor no existe en la bbd");
            }

            context.Update(articuloDtoNuevo);
           await context.SaveChangesAsync();

            return Ok(articuloDtoNuevo);
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Articulo>> DeleteArticulo(int id)
        {

            var existe = await context.Articulos.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound("El id de autor no existe en la bbd");
            }
            context.Remove(new Articulo() { Id = id });

            await context.SaveChangesAsync();
            return Ok("Eliminado correctamente");
        }
    }
}
