using System.ComponentModel.DataAnnotations;

namespace TecnologiaDigitalLK.API.DTOs.Categorias
{
    public class CategoriaCreateDto
    {
        [Required(ErrorMessage = "El Nombre es Obligatorio")]
        public string Nombre { get; set; } = string.Empty;
    }
}
