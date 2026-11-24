using AutoMapper;
using TecnologiaDigitalLK.API.DTOs.Usuarios;
using TecnologiaDigitalLK.API.Modelos;

namespace TecnologiaDigitalLK.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //USUARIO//

            //Mapeo de UsuarioCreateDto --> Usuario
            CreateMap<UsuarioCreateDto, Usuario>();
            //Mapeo de Usuario --> UsuarioReadDto
            CreateMap<Usuario, UsuarioReadDto>()
                .ForMember(dest => dest.SucursalNombre, opt => opt.MapFrom(src => src.Sucursal != null ? src.Sucursal.Nombre : "Global"));
            //Mapeo de UsuarioUpdate --> Usuario
            CreateMap<UsuarioUpdateDto, Usuario>();

        }
    }
}
