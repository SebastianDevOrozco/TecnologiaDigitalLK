namespace TecnologiaDigitalLK.API.Modelos
{
    public class Gasto
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; } = DateTime.UtcNow;

        public int SucursalId { get; set; }
        public Sucursal? Sucursal { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public string Descripcion { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public bool Activo { get; set; } = true;
    }
}
