using System.ComponentModel.DataAnnotations;

namespace JwtTeach.Dto
{
    public class Credenciales
    {

        [Required]
       
        public string Nombre { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
