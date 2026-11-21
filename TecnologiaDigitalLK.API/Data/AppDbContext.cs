using Microsoft.EntityFrameworkCore;
using TecnologiaDigitalLK.API.Modelos;
namespace TecnologiaDigitalLK.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Tablas
        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<StockSucursal> StocksSucursal { get; set; }
        public DbSet<MetodoDePago> MetodosPago { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetallesVenta { get; set; }
        public DbSet<Traslado> Traslados { get; set; }
        public DbSet<DetalleTraslado> DetallesTraslado { get; set; }
        public DbSet<Gasto> Gastos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // CONFIGURACION DE DECIMALES (Precios, costos y montos en MySQL)
            modelBuilder.Entity<Producto>().Property(p => p.CostoAdquisicion).HasPrecision(18, 2);
            modelBuilder.Entity<Producto>().Property(p => p.PrecioDetal).HasPrecision(18, 2);
            modelBuilder.Entity<Producto>().Property(p => p.PrecioMayor).HasPrecision(18, 2);

            modelBuilder.Entity<Venta>().Property(v => v.TotalVenta).HasPrecision(18, 2);
            modelBuilder.Entity<Venta>().Property(v => v.UtilidadNetaTotal).HasPrecision(18, 2);

            modelBuilder.Entity<DetalleVenta>().Property(d => d.CostoHistorico).HasPrecision(18, 2);
            modelBuilder.Entity<DetalleVenta>().Property(d => d.PrecioVendido).HasPrecision(18, 2);
            modelBuilder.Entity<DetalleVenta>().Property(d => d.Subtotal).HasPrecision(18, 2);
            modelBuilder.Entity<DetalleVenta>().Property(d => d.UtilidadNeta).HasPrecision(18, 2);

            modelBuilder.Entity<Gasto>().Property(g => g.Monto).HasPrecision(18, 2);

            // INDICES UNICOS
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Producto>()
                .HasIndex(p => p.Codigo)
                .IsUnique();

            modelBuilder.Entity<StockSucursal>()
                .HasIndex(s => new { s.ProductoId, s.SucursalId })
                .IsUnique();

            // RELACIONES

            // Relacion Sucursal --> Usuario
            modelBuilder.Entity<Sucursal>()
                .HasMany(s => s.Usuarios)
                .WithOne(u => u.Sucursal)
                .HasForeignKey(u => u.SucursalId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacion Sucursal --> StockSucursal
            modelBuilder.Entity<Sucursal>()
                .HasMany(s => s.StocksSucursal)
                .WithOne(st => st.Sucursal)
                .HasForeignKey(st => st.SucursalId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacion Sucursal --> Venta
            modelBuilder.Entity<Sucursal>()
                .HasMany(s => s.Ventas)
                .WithOne(v => v.Sucursal)
                .HasForeignKey(v => v.SucursalId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacion Sucursal --> Gasto
            modelBuilder.Entity<Sucursal>()
                .HasMany(s => s.Gastos)
                .WithOne(g => g.Sucursal)
                .HasForeignKey(g => g.SucursalId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacion Sucursal --> Traslado (Origen)
            modelBuilder.Entity<Sucursal>()
                .HasMany(s => s.TrasladosOrigen)
                .WithOne(t => t.SucursalOrigen)
                .HasForeignKey(t => t.SucursalOrigenId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacion Sucursal --> Traslado (Destino)
            modelBuilder.Entity<Sucursal>()
                .HasMany(s => s.TrasladosDestino)
                .WithOne(t => t.SucursalDestino)
                .HasForeignKey(t => t.SucursalDestinoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacion Categoria --> Producto
            modelBuilder.Entity<Categoria>()
                .HasMany(c => c.Productos)
                .WithOne(p => p.Categoria)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacion Producto --> StockSucursal
            modelBuilder.Entity<Producto>()
                .HasMany(p => p.StocksSucursal)
                .WithOne(st => st.Producto)
                .HasForeignKey(st => st.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacion Producto --> DetalleVenta
            modelBuilder.Entity<Producto>()
                .HasMany(p => p.DetallesVenta)
                .WithOne(dv => dv.Producto)
                .HasForeignKey(dv => dv.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacion Producto --> DetalleTraslado
            modelBuilder.Entity<Producto>()
                .HasMany(p => p.DetallesTraslado)
                .WithOne(dt => dt.Producto)
                .HasForeignKey(dt => dt.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacion Usuario --> Venta
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Ventas)
                .WithOne(v => v.Usuario)
                .HasForeignKey(v => v.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacion Usuario --> Traslado
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Traslados)
                .WithOne(t => t.Usuario)
                .HasForeignKey(t => t.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacion Usuario --> Gasto
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Gastos)
                .WithOne(g => g.Usuario)
                .HasForeignKey(g => g.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacion MetodoPago --> Venta
            modelBuilder.Entity<MetodoDePago>()
                .HasMany(m => m.Ventas)
                .WithOne(v => v.MetodoPago)
                .HasForeignKey(v => v.MetodoPagoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacion Venta --> DetalleVenta
            modelBuilder.Entity<Venta>()
                .HasMany(v => v.DetallesVenta)
                .WithOne(dv => dv.Venta)
                .HasForeignKey(dv => dv.VentaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacion Traslado --> DetalleTraslado
            modelBuilder.Entity<Traslado>()
                .HasMany(t => t.DetallesTraslado)
                .WithOne(dt => dt.Traslado)
                .HasForeignKey(dt => dt.TrasladoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

