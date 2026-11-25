using TecnologiaDigitalLK.API.Modelos;

namespace TecnologiaDigitalLK.API.DTOs.Productos
{
    public class ProductoReadDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string CategoriaNombre { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string? ImagenRuta { get; set; }
        public decimal CostoAdquisicion { get; set; }
        public decimal PrecioDetal { get; set; }
        public decimal PrecioMayor { get; set; }
    }
}
