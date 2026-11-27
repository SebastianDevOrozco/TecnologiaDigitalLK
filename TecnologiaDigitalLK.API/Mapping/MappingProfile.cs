using AutoMapper;
using TecnologiaDigitalLK.API.DTOs.Categorias;
using TecnologiaDigitalLK.API.DTOs.Productos;
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


            //CATEGORIA//
            //Mapeo de CategoriaCreateDto --> Categoria
            CreateMap<CategoriaCreateDto, Categoria>();
            //Mapeo de Categoria --> CategoriaReadDto
            CreateMap<Categoria, CategoriaReadDto>();
            //Mapeo de CategoriaUpdateDto --> Categoria
            CreateMap<CategoriaUpdateDto, Categoria>();

            //PRODUCTO
            CreateMap<ProductoCreateDto, Producto>()
                .ForMember(dest => dest.ImagenRuta, opt => opt.Ignore());

            CreateMap<Producto, ProductoReadDto>();

        }
    }
}
