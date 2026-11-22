using TecnologiaDigitalLK.API.Modelos;

namespace TecnologiaDigitalLK.API.Data
{
    public class DbSedder
    {
        public static void Seed(AppDbContext context)
        {
            // 1. Sembrar Sucursales iniciales
            if (!context.Sucursales.Any())
            {
                context.Sucursales.AddRange(
                    new Sucursal
                    {
                        Nombre = "Sede Principal",
                        Direccion = "san andrexitos local 2003",
                        Telefono = "3118478795",
                        Activo = true
                    },
                    new Sucursal
                    {
                        Nombre = "Sede Secundaria",
                        Direccion = "san andrexitos local 2039",
                        Telefono = "3118478795",
                        Activo = true
                    }
                );
                context.SaveChanges();
            }

            // 2. Sembrar Métodos de Pago
            if (!context.MetodosPago.Any())
            {
                context.MetodosPago.AddRange(
                    new MetodoDePago { Nombre = "Efectivo", EsEfectivo = true, Activo = true },
                    new MetodoDePago { Nombre = "Nequi", EsEfectivo = false, Activo = true },
                    new MetodoDePago { Nombre = "Bancolombia", EsEfectivo = false, Activo = true },
                    new MetodoDePago { Nombre = "Daviplata", EsEfectivo = false, Activo = true },
                    new MetodoDePago { Nombre = "Datafono / Tarjeta", EsEfectivo = false, Activo = true }
                );
                context.SaveChanges();
            }

            // 3. Sembrar Usuario Administrador Global (SucursalId = null)
            if (!context.Usuarios.Any())
            {
                context.Usuarios.Add(new Usuario
                {
                    Nombre = "Administrador General Luz Karime",
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123*"),
                    Rol = "Admin",
                    SucursalId = null,
                    Activo = true
                });
                context.SaveChanges();
            }
        }
    }
}

