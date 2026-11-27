using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TecnologiaDigitalLK.API.Data;
using TecnologiaDigitalLK.API.DTOs.Productos;
using TecnologiaDigitalLK.API.Interfaces.Imagenes;
using TecnologiaDigitalLK.API.Interfaces.Productos;
using TecnologiaDigitalLK.API.Modelos;

namespace TecnologiaDigitalLK.API.Servicios.Productos
{
    public class ProductoService : IProductoService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IImagenService _imagenService;

        public ProductoService(AppDbContext context, IMapper mapper, IImagenService imagenService)
        {
            _appDbContext = context;
            _mapper = mapper;
            _imagenService = imagenService;
        }

        public async Task<ProductoReadDto> CrearProductoAsync(ProductoCreateDto productoDto)
        {
            // 1 validaciones 
            await ValidarNombreDeProducto(productoDto.Nombre);
            await ValidarCategoria(productoDto.CategoriaId);
            ValidacionDeCostos(productoDto); // sin await porque es un metodo que no consulta a la DB 
            var sucursales = await ValidarSucursales();

            // 2. Subir imagen a Cloudinary (si se envió archivo)
            string? imagenUrl = null;
            string? imagenPublicId = null;

            if (productoDto.ImagenRuta != null)
            {
                (imagenUrl, imagenPublicId) = await _imagenService.SubirImagenAsync(productoDto.ImagenRuta, "productos");
            }

            // 3. Transacción en MySQL: Producto + Stock inicial por sucursal
            await using var transaction = await _appDbContext.Database.BeginTransactionAsync();

            try
            {
                // Crear producto
                var producto = _mapper.Map<Producto>(productoDto);
                producto.ImagenRuta = imagenUrl;
                producto.ImagenPublicId = imagenPublicId;

                _appDbContext.Productos.Add(producto);
                await _appDbContext.SaveChangesAsync();

                // Crear registros iniciales en StockSucursal (Local 1 y Local 2)
                var stocksIniciales = sucursales.Select(s => new StockSucursal
                {
                    ProductoId = producto.Id,
                    SucursalId = s.Id,
                    CantidadActual = 0
                }).ToList();

                _appDbContext.StocksSucursal.AddRange(stocksIniciales);
                await _appDbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                // Cargar el nombre de la categoria
                await _appDbContext.Entry(producto).Reference(p => p.Categoria).LoadAsync();

                return _mapper.Map<ProductoReadDto>(producto);
            }
            catch
            {
                await transaction.RollbackAsync();

                if (!string.IsNullOrEmpty(imagenPublicId))
                {
                    await _imagenService.EliminarImagenAsync(imagenPublicId);
                }
                throw;
            }
        }

        public async Task<ProductoReadDto> ObtenerProductoPorIdAsync(int id)
        {
            var producto = await _appDbContext.Productos
                .AsNoTracking()
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
                throw new KeyNotFoundException($"No se encontró el producto con ID {id}.");

            return _mapper.Map<ProductoReadDto>(producto);
        }



        private async Task ValidarNombreDeProducto(string nombre)
        {
            var nombreExiste = await _appDbContext.Productos
                 .AnyAsync(p => p.Nombre.ToLower() == nombre.Trim().ToLower());

            if (nombreExiste)
                throw new ArgumentException($"Ya existe un producto con el nombre '{nombre}'.");
        }
        private async Task ValidarCategoria(int categoriaId)
        {
            var categoriaExiste = await _appDbContext.Categorias
                .AnyAsync(c => c.Id == categoriaId && c.Activo);

            if (!categoriaExiste)
                throw new KeyNotFoundException($"No existe una categoría activa con ID {categoriaId}.");
        }
        private void ValidacionDeCostos(ProductoCreateDto dto)
        {
            if (dto.PrecioDetal < dto.CostoAdquisicion || dto.PrecioMayor < dto.CostoAdquisicion)
                throw new ArgumentException("Los precios de venta no pueden ser menores al costo de adquisición.");

            if (dto.PrecioDetal < dto.PrecioMayor)
                throw new ArgumentException("El precio al detal no puede ser menor al precio al por mayor.");
        }

        private async Task<List<Sucursal>> ValidarSucursales()
        {
            var sucursales = await _appDbContext.Sucursales.ToListAsync();

            if (sucursales.Count == 0)
                throw new InvalidOperationException("No hay sucursales registradas en el sistema para inicializar el stock.");

            return sucursales;
        }
    }
}
