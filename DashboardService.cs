using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FerreAppLaVarilla.UI.Data;
using FerreAppLaVarilla.UI.Models;

namespace FerreAppLaVarilla.UI.Services
{
    // --- MODELOS UNIFICADOS PARA GRÁFICOS ---
    public record VentaPorDia(string FechaLabel, double Monto, DateTime FechaReal);
    public record VentaPorCategoria(string Categoria, double Monto, double Porcentaje);
    public record ProductoMasVendido(string Nombre, string Categoria, int Cantidad, double Monto);
    public record PedidoReciente(string Id, string Cliente, string Estado, double Total);

    public class DashboardStats
    {
        public int TotalProductos { get; set; }
        public int TotalUsuarios { get; set; }
        public int StockTotal { get; set; }
        public decimal InversionTotal { get; set; }
        public double VentasHoy { get; set; }
        public int PedidosPendientes { get; set; }
        public int ClientesRegistrados { get; set; }

        public List<VentaPorDia> HistoricoVentas { get; set; } = new();
        public List<VentaPorCategoria> VentasPorCategoria { get; set; } = new();
        public List<ProductoMasVendido> Top10Productos { get; set; } = new();
        public List<PedidoReciente> UltimosPedidos { get; set; } = new();

        public double TendenciaVentas { get; set; }
        public double TendenciaPedidos { get; set; }
    }

    public class DashboardService
    {
        private readonly ApplicationDbContext _context;
        public DashboardService(ApplicationDbContext context) => _context = context;

        public async Task<DashboardStats> ObtenerEstadisticasAsync(int diasAtras = 7)
        {
            var hoy = DateTime.Today;
            var fechaInicio = hoy.AddDays(-(diasAtras - 1));

            // 1. Inventario e Inversión
            var stockTotal = await _context.Productos.SumAsync(p => (int?)p.Stock) ?? 0;
            var productosInv = await _context.Productos.Select(p => new { p.PrecioCompra, p.Stock }).ToListAsync();
            decimal inversionTotal = productosInv.Sum(p => p.PrecioCompra * p.Stock);

            // 2. Usuarios y Pedidos Pendientes
            int totalClientes = await _context.Usuarios.CountAsync(u => u.Rol == "Cliente");
            int pendientes = await _context.Pedidos.CountAsync(p => p.Estado == EstadoPedido.Pendiente);
            int pedidosHoy = await _context.Pedidos.CountAsync(p => p.FechaCreacion >= hoy);

            // 3. Ventas Reales (Ya usamos la columna Total de verdad)
            double ventasHoy = (double)(await _context.Pedidos
                .Where(p => p.FechaCreacion >= hoy)
                .SumAsync(p => (decimal?)p.Total) ?? 0);

            double ventasAyer = (double)(await _context.Pedidos
                .Where(p => p.FechaCreacion >= hoy.AddDays(-1) && p.FechaCreacion < hoy)
                .SumAsync(p => (decimal?)p.Total) ?? 0);

            // 4. Últimos Pedidos (Con el Total y el Correo real)
            var ultimos = await _context.Pedidos
                .Include(p => p.Cliente)
                .OrderByDescending(p => p.FechaCreacion)
                .Take(5)
                .Select(p => new PedidoReciente(
                    "PED-" + p.Id,
                    p.Cliente != null ? p.Cliente.CorreoElectronico : "Consumidor Final",
                    p.Estado.ToString(),
                    (double)p.Total
                ))
                .ToListAsync();

            // 5. Histórico de Ventas (Para el gráfico de líneas)
            var ventasRaw = await _context.Pedidos
                .Where(p => p.FechaCreacion >= fechaInicio)
                .Select(p => new { p.FechaCreacion.Date, p.Total })
                .ToListAsync();

            var historico = Enumerable.Range(0, diasAtras)
                .Select(offset => {
                    var f = fechaInicio.AddDays(offset);
                    var totalDia = ventasRaw.Where(v => v.Date == f.Date).Sum(v => (double)v.Total);
                    return new VentaPorDia(f.ToString("dd MMM"), totalDia, f);
                }).ToList();

            // 6. Datos para Gráfico de Pastel y Barras (Top Productos)
            // Usamos Set<DetallePedido>() para evitar problemas de pluralización
            var detallesRaw = await _context.Set<DetallePedido>()
                .Include(dp => dp.Producto)
                .Include(dp => dp.Pedido)
                .Where(dp => dp.Pedido != null && dp.Pedido.FechaCreacion >= fechaInicio)
                .Select(dp => new {
                    dp.ProductoId,
                    NombreProducto = dp.Producto.Nombre,
                    CategoriaProducto = dp.Producto.Categoria,
                    dp.Cantidad,
                    MontoLineal = (double)(dp.PrecioUnitario * dp.Cantidad)
                })
                .ToListAsync();

            double totalMontoDetalles = detallesRaw.Sum(d => d.MontoLineal);

            var ventasPorCategoria = detallesRaw
                .GroupBy(d => d.CategoriaProducto ?? "General")
                .Select(g => new VentaPorCategoria(
                    g.Key,
                    g.Sum(d => d.MontoLineal),
                    totalMontoDetalles > 0 ? (g.Sum(d => d.MontoLineal) / totalMontoDetalles) * 100 : 0
                ))
                .OrderByDescending(v => v.Porcentaje)
                .ToList();

            var top10Productos = detallesRaw
                .GroupBy(dp => new { dp.ProductoId, dp.NombreProducto, dp.CategoriaProducto })
                .Select(g => new ProductoMasVendido(
                    g.Key.NombreProducto ?? "Desconocido",
                    g.Key.CategoriaProducto ?? "General",
                    g.Sum(dp => dp.Cantidad),
                    g.Sum(dp => dp.MontoLineal)
                ))
                .OrderByDescending(p => p.Cantidad)
                .Take(10)
                .ToList();

            return new DashboardStats
            {
                TotalProductos = await _context.Productos.CountAsync(),
                StockTotal = stockTotal,
                InversionTotal = inversionTotal,
                VentasHoy = ventasHoy,
                PedidosPendientes = pendientes,
                ClientesRegistrados = totalClientes,
                UltimosPedidos = ultimos,
                HistoricoVentas = historico,
                VentasPorCategoria = ventasPorCategoria,
                Top10Productos = top10Productos,

                // Calculamos el porcentaje de crecimiento
                TendenciaVentas = ventasAyer > 0 ? ((ventasHoy - ventasAyer) / ventasAyer) * 100 : (ventasHoy > 0 ? 100 : 0),
                TendenciaPedidos = pendientes > 0 ? ((double)(pedidosHoy - pendientes) / pendientes) * 100 : (pedidosHoy > 0 ? 100 : 0)
            };
        }
    }
}