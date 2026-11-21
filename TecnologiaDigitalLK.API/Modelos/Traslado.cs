namespace TecnologiaDigitalLK.API.Modelos
{
    public class Traslado
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; } = DateTime.UtcNow;

        public int SucursalOrigenId { get; set; }
        public Sucursal? SucursalOrigen { get; set; }

        public int SucursalDestinoId { get; set; }
        public Sucursal? SucursalDestino { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public string? Observacion { get; set; }

        public ICollection<DetalleTraslado> DetallesTraslado { get; set; } = new List<DetalleTraslado>();
    }
}
