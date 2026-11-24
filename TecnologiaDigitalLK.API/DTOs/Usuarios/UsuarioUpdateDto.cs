using System.ComponentModel.DataAnnotations;

namespace TecnologiaDigitalLK.API.DTOs.Usuarios
{
    public class UsuarioUpdateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;

        public int? SucursalId { get; set; }
    }
}
