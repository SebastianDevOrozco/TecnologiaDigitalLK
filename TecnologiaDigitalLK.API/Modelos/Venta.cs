namespace TecnologiaDigitalLK.API.Modelos
{
    public class Venta
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; } = DateTime.UtcNow;

        public int SucursalId { get; set; }
        public Sucursal? Sucursal { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public int MetodoPagoId { get; set; }
        public MetodoDePago? MetodoPago { get; set; }

        public decimal TotalVenta { get; set; }
        public decimal UtilidadNetaTotal { get; set; }
        public bool Activo { get; set; } = true;

        public ICollection<DetalleVenta> DetallesVenta { get; set; } = new List<DetalleVenta>();
    }
}
