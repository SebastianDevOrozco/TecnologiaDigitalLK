using System.ComponentModel.DataAnnotations;

namespace TecnologiaDigitalLK.API.DTOs.Usuarios
{
    public class UsuarioLoginDto
    {
        [Required(ErrorMessage = "El Usuario es Obligatorio")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "La Contraseña es Obligatorio")]
        public string PasswordHash { get; set; } = string.Empty;
    }
}
