using System.ComponentModel.DataAnnotations.Schema;

namespace TecnologiaDigitalLK.API.Modelos
{
    public class Sucursal
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public bool Activo { get; set; } = true;

        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public ICollection<StockSucursal> StocksSucursal { get; set; } = new List<StockSucursal>();
        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
        public ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();

        [InverseProperty("SucursalOrigen")]
        public ICollection<Traslado> TrasladosOrigen { get; set; } = new List<Traslado>();

        [InverseProperty("SucursalDestino")]
        public ICollection<Traslado> TrasladosDestino { get; set; } = new List<Traslado>();
    }
}
