using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TecnologiaDigitalLK.API.DTOs.Categorias;
using TecnologiaDigitalLK.API.Interfaces.Categorias;

namespace TecnologiaDigitalLK.API.Controllers.Categorias
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoriaReadDto>> CrearCategoria(CategoriaCreateDto createDto)
        {
            var categoria = await _categoriaService.CrearCategoria(createDto);
            return Ok(categoria);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaReadDto>>> ListaCategorias()
        {
            var categorias = await _categoriaService.ListarCategorias();
            return Ok(categorias);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaReadDto>> ObtenerCategoriaPorId(int id)
        {
            var categoria = await _categoriaService.ObtenerCategoriaPorIdAsync(id);
            return CreatedAtAction(nameof(ObtenerCategoriaPorId), new { id = categoria.Id }, categoria);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoriaReadDto>> ActualizarCategoria(int id, CategoriaUpdateDto updateDto)
        {
            var categoriaActualizada = await _categoriaService.ActualizarCategoria(id, updateDto);
            return Ok(categoriaActualizada);
        }

        [HttpPatch("{id}/cambiar-estado")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CambioEstado(int id, bool nuevoEstado)
        {
            await _categoriaService.EditarEstadoCategoria(id, nuevoEstado);
            return NoContent();
        }
    }
}
