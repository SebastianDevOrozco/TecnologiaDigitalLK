using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TecnologiaDigitalLK.API.DTOs.Usuarios;
using TecnologiaDigitalLK.API.Interfaces.Usuarios;

namespace TecnologiaDigitalLK.API.Controllers.Usuarios
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UsuarioReadDto>> ActualizarUsuario(int id, UsuarioUpdateDto usuarioDto)
        {
            var usuario = await _usuarioService.ActualizarUsuario(id, usuarioDto);
            return Ok(usuario);
        }

        [HttpPatch("{id}/cambiar-estado")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CambioEstado(int id, bool nuevoEstado)
        {
            await _usuarioService.CambiarEstadoUsuario(id, nuevoEstado);
            return NoContent();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UsuarioReadDto>>> ListarUsuarios()
        {
            var usuarios = await _usuarioService.ListarUsuarios();
            return Ok(usuarios);
        }


    }
}
