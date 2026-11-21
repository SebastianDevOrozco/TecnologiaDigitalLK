namespace TecnologiaDigitalLK.API.Modelos
{
    public class Producto
    {
        public int Id { get; set; }
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? ImagenRuta { get; set; }

        public decimal CostoAdquisicion { get; set; }
        public decimal PrecioDetal { get; set; }
        public decimal PrecioMayor { get; set; }
        public bool Activo { get; set; } = true;

        public ICollection<StockSucursal> StocksSucursal { get; set; } = new List<StockSucursal>();
        public ICollection<DetalleVenta> DetallesVenta { get; set; } = new List<DetalleVenta>();
        public ICollection<DetalleTraslado> DetallesTraslado { get; set; } = new List<DetalleTraslado>();
    }
}
