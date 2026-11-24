using TecnologiaDigitalLK.API.DTOs.Categorias;

namespace TecnologiaDigitalLK.API.Interfaces.Categorias
{
    public interface ICategoriaService
    {
        //CREAR
        Task<CategoriaReadDto> CrearCategoria(CategoriaCreateDto categoriaCreateDto);

        //LEER
        Task<IEnumerable<CategoriaReadDto>> ListarCategorias();

        //ACTUALIZAR
        Task<CategoriaReadDto> ActualizarCategoria(int categoriaId, CategoriaUpdateDto categoriaUpdateDto);

        //PATCH
        Task EditarEstadoCategoria(int categoriaId, bool nuevoEstado);
    }
}
