using System.ComponentModel.DataAnnotations;

namespace TecnologiaDigitalLK.API.DTOs.Productos
{
    public class ProductoUpdateDto
    {
        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una categoría válida.")]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 150 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        public IFormFile? ImagenRuta { get; set; }

        [Required(ErrorMessage = "El costo de adquisición es obligatorio.")]
        [Range(0.01, 999999999.99, ErrorMessage = "El costo de adquisición debe ser mayor a 0.")]
        public decimal CostoAdquisicion { get; set; }

        [Required(ErrorMessage = "El precio al detal es obligatorio.")]
        [Range(0.01, 999999999.99, ErrorMessage = "El precio al detal debe ser mayor a 0.")]
        public decimal PrecioDetal { get; set; }

        [Required(ErrorMessage = "El precio al por mayor es obligatorio.")]
        [Range(0.01, 999999999.99, ErrorMessage = "El precio al por mayor debe ser mayor a 0.")]
        public decimal PrecioMayor { get; set; }
    }
}
