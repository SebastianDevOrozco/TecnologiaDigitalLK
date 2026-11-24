using TecnologiaDigitalLK.API.DTOs.Usuarios;

namespace TecnologiaDigitalLK.API.Interfaces.Usuarios
{
    public interface IAuthService
    {
        Task<UsuarioReadDto> RegistrarUsuario(UsuarioCreateDto usuarioCreateDto);
        Task<string?> Login(string UserName, string Password);
    }
}
