using System.ComponentModel.DataAnnotations;

namespace TecnologiaDigitalLK.API.DTOs.Usuarios
{
    public class UsuarioCreateDto
    {
        [Required(ErrorMessage = "El Nombre es Obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El Username es Obligatorio")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "La Contraseña es Obligatoria")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        public string PasswordHash { get; set; } = string.Empty;

        [Required(ErrorMessage = "El Rol es Obligatorio")]
        public string Rol { get; set; } = string.Empty;

        public int? SucursalId { get; set; }
    }
}
