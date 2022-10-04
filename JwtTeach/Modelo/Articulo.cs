using Microsoft.AspNetCore.Identity;

namespace JwtTeach.Modelo
{
    public class Articulo
    {
        public int Id { get; set; }
        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        // relacion con el usuario 
        public string? UsuarioId { get; set; }

        public IdentityUser Usuario{ get; set; }
    }
}
