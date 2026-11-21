namespace TecnologiaDigitalLK.API.Modelos
{
    public class MetodoDePago
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty; // Efectivo, Nequi, Bancolombia, etc.
        public bool EsEfectivo { get; set; }
        public bool Activo { get; set; } = true;

        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
    }
}
