using TecnologiaDigitalLK.API.DTOs.Usuarios;

namespace TecnologiaDigitalLK.API.Interfaces.Usuarios
{
    public interface IUsuarioService
    {
        Task<UsuarioReadDto> ActualizarUsuario(int usuarioId, UsuarioUpdateDto usuarioDto);
        Task CambiarEstadoUsuario(int usuarioId, bool nuevoEstado);
        Task <IEnumerable<UsuarioReadDto>> ListarUsuarios();
    }
}
