namespace TecnologiaDigitalLK.API.Modelos
{
    public class DetalleVenta
    {
        public int Id { get; set; }
        public int VentaId { get; set; }
        public Venta? Venta { get; set; }

        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        public int Cantidad { get; set; }
        public string TipoTarifa { get; set; } = "Detal"; // "Detal" o "Mayor"
        public decimal CostoHistorico { get; set; }
        public decimal PrecioVendido { get; set; }
        public decimal Subtotal { get; set; }
        public decimal UtilidadNeta { get; set; }
    }
}
