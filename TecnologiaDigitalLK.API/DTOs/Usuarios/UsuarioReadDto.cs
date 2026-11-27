namespace TecnologiaDigitalLK.API.DTOs.Usuarios
{
    public class UsuarioReadDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public string SucursalNombre { get; set; } = string.Empty;
    }
}
