using AutoMapper;
using JwtTeach.Dto;
using JwtTeach.Modelo;

namespace JwtTeach.Utilidades
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreaArticuloDTO, Articulo>();
        }
     
    }
}
