namespace TecnologiaDigitalLK.API.Modelos
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Rol { get; set; } = "Cajero"; // "Admin" o "Cajero"

        public int? SucursalId { get; set; }
        public Sucursal? Sucursal { get; set; }

        public bool Activo { get; set; } = true;

        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
        public ICollection<Traslado> Traslados { get; set; } = new List<Traslado>();
        public ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();
    }
}
