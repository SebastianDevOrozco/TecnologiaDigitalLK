# Modelo Relacional - Tecnología Digital LK

La base de datos está compuesta por **11 entidades** diseñadas bajo integridad referencial estricta (`DeleteBehavior.Restrict`) para proteger el historial contable y de inventario entre sucursales.

## Tabla de Relaciones

| Entidad Principal (1) | Entidad Dependiente (N) | Llave Foránea | Cardinalidad | Regla de Negocio / Descripción |
| :--- | :--- | :--- | :--- | :--- |
| **Sucursal** | **Usuario** | `SucursalId` *(nullable)* | `1 : N` | Una sede tiene varios cajeros; un cajero pertenece a una sede. Si es `null`, es **Admin global** con acceso a todas las sedes. |
| **Sucursal** | **StockSucursal** | `SucursalId` | `1 : N` | Cada sede gestiona su propio inventario independiente por producto. |
| **Sucursal** | **Venta** | `SucursalId` | `1 : N` | Toda venta queda asociada a la sede donde se facturó. |
| **Sucursal** | **Gasto** | `SucursalId` | `1 : N` | Los egresos operativos se registran por sede para calcular la utilidad real de cada local. |
| **Sucursal** | **Traslado** *(Origen)* | `SucursalOrigenId` | `1 : N` | Sede desde la cual sale la mercancía en un traslado. |
| **Sucursal** | **Traslado** *(Destino)* | `SucursalDestinoId` | `1 : N` | Sede que recibe la mercancía en un traslado. |
| **Categoria** | **Producto** | `CategoriaId` | `1 : N` | Una categoría agrupa múltiples productos; cada producto pertenece a una categoría. |
| **Producto** | **StockSucursal** | `ProductoId` | `1 : N` | Un producto del catálogo tiene un registro de existencias y stock mínimo por cada sede. |
| **Producto** | **DetalleVenta** | `ProductoId` | `1 : N` | Un producto puede incluirse en múltiples líneas de detalle de distintas ventas. |
| **Producto** | **DetalleTraslado** | `ProductoId` | `1 : N` | Un producto puede incluirse en múltiples movimientos de traslado entre sedes. |
| **Usuario** | **Venta** | `UsuarioId` | `1 : N` | Registra qué empleado o administrador realizó cada venta. |
| **Usuario** | **Traslado** | `UsuarioId` | `1 : N` | Registra quién autorizó o ejecutó el movimiento de inventario. |
| **Usuario** | **Gasto** | `UsuarioId` | `1 : N` | Registra qué usuario reportó el gasto en caja. |
| **MetodoPago** | **Venta** | `MetodoPagoId` | `1 : N` | Cada venta se liquida con un medio de pago (Efectivo, Nequi, Bancolombia, etc.). |
| **Venta** | **DetalleVenta** | `VentaId` | `1 : N` | Una cabecera de venta agrupa uno o varios productos vendidos con su precio y costo histórico. |
| **Traslado** | **DetalleTraslado** | `TrasladoId` | `1 : N` | Una cabecera de traslado agrupa uno o varios productos enviados entre sedes. |

## Resolución de Relaciones Muchos a Muchos (`N : M`)

Para evitar redundancia y guardar datos históricos por operación, las relaciones `N : M` se resuelven mediante tres tablas intermedias:

* **Sucursal ↔ Producto** se resuelve con **`StockSucursal`** (almacena `CantidadActual` y `StockMinimo` por sede).
* **Venta ↔ Producto** se resuelve con **`DetalleVenta`** (almacena `Cantidad`, `TipoTarifa`, `CostoHistorico` y `PrecioVendido`).
* **Traslado ↔ Producto** se resuelve con **`DetalleTraslado`** (almacena `Cantidad` enviada).