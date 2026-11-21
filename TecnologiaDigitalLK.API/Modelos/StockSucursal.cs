namespace TecnologiaDigitalLK.API.Modelos
{
    public class StockSucursal
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        public int SucursalId { get; set; }
        public Sucursal? Sucursal { get; set; }

        public int CantidadActual { get; set; }
        public int StockMinimo { get; set; }
    }
}
