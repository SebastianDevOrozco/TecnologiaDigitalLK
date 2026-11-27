using TecnologiaDigitalLK.API.DTOs.Productos;

namespace TecnologiaDigitalLK.API.Interfaces.Productos
{
    public interface IProductoService
    {
        Task<ProductoReadDto> CrearProductoAsync(ProductoCreateDto productoDto);
        Task<ProductoReadDto> ObtenerProductoPorIdAsync(int id);
    }
}
