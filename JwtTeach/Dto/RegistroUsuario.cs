using System.ComponentModel.DataAnnotations;

namespace JwtTeach.Dto
{
    public class RegistroUsuario
    {

        public string Nombre { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
