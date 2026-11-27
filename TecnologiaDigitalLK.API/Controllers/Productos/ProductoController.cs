using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TecnologiaDigitalLK.API.DTOs.Productos;
using TecnologiaDigitalLK.API.Interfaces.Productos;

namespace TecnologiaDigitalLK.API.Controllers.Productos
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductoController : Controller
    {
        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductoReadDto>> CrearProducto([FromForm] ProductoCreateDto dto)
        {
            var productoCreado = await _productoService.CrearProductoAsync(dto);
            return CreatedAtAction(nameof(CrearProducto), new { id = productoCreado.Id }, productoCreado);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoReadDto>> ObtenerProductoPorId(int id)
        {
            var producto = await _productoService.ObtenerProductoPorIdAsync(id);
            return Ok(producto);
        }
    }
}
