using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TecnologiaDigitalLK.API.DTOs.Usuarios;
using TecnologiaDigitalLK.API.Interfaces.Usuarios;

namespace TecnologiaDigitalLK.API.Controllers.Usuarios
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Registrar")]
        public async Task<ActionResult<UsuarioReadDto>> Registrar(UsuarioCreateDto usuarioCreateDto)
        {

            var resultado = await _authService.RegistrarUsuario(usuarioCreateDto);
            return Ok(resultado);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDto loginDto)
        {
            var token = await _authService.Login(loginDto.Username, loginDto.PasswordHash);
            return Ok(new { token });
        }
    }
}
