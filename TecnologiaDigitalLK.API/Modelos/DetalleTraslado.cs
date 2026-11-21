namespace TecnologiaDigitalLK.API.Modelos
{
    public class DetalleTraslado
    {
        public int Id { get; set; }
        public int TrasladoId { get; set; }
        public Traslado? Traslado { get; set; }

        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        public int Cantidad { get; set; }
    }
}
