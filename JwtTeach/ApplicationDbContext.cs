

using JwtTeach.Modelo;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JwtTeach
{
    //public class ApplicationDbContext:DbContext
   public class ApplicationDbContext : IdentityDbContext
    {

       

        public ApplicationDbContext(DbContextOptions options):base(options)
        {

        }

       
        public DbSet<Articulo> Articulos { get; set; }


    }
}
