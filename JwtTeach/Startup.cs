

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace JwtTeach
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            //limpia el mapeo funciona cuando metemos el email
            //limipa el matpeo que ocurre en los claims
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //services
        public void ConfigureServices(IServiceCollection services)
        {

            //=== lo que creamos el startup===============================
            services.AddControllers()
                .AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();


           // services.AddSwaggerGen();


            //============================================================================

            // conexion a slq
            services.AddDbContext<ApplicationDbContext>(option =>
                option.UseSqlServer(Configuration.GetConnectionString("Conexion")));



            ///para la jwt
            ///
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               //verifica que haya firmado correctamente con la llave que esta en el appsetting.json
               .AddJwtBearer(opciones => opciones.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
               {
                   ValidateIssuer = false,
                   ValidateAudience = false,
                    //validar el tiempo de vida
                    ValidateLifetime = true,
                    //validar firma
                    ValidateIssuerSigningKey = true,
                    //configuramos la llave
                    IssuerSigningKey = new SymmetricSecurityKey(
                       Encoding.UTF8.GetBytes(Configuration["llave"])),
                   ClockSkew = TimeSpan.Zero
               });

            services.AddSwaggerGen(
                //para mandar el token por swagger
                //mandar por el authorize de Swagger = Bearer CodigoTokenLogin
                c =>
                {
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference =new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            new string[]{}
                        }
                    });
                }

                );


            services.AddIdentity<IdentityUser, IdentityRole>()
              .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //automapper configuracion
            services.AddAutoMapper(typeof(Startup));





            // esto es para Identity para el servicio de identity de token

            //=================================================

        }

        //midlewares

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {

            //=== lo que creamos el startup===============================
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseRouting();

          

           app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //=== fin lo que creamos el startup===============================






        }
    }
}
