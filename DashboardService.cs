using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FerreAppLaVarilla.UI.Data;
using FerreAppLaVarilla.UI.Models;
using System.Threading;

namespace FerreAppLaVarilla.UI.Services
{
    // ==========================================================
    // 1. LOS MODELOS DE DATOS (Estos son los que faltaban)
    // ==========================================================
    public record VentaPorDia(string FechaLabel, decimal Monto, DateTime FechaReal);
    public record VentaPorCategoria(string Categoria, decimal Monto, decimal Porcentaje);
    public record ProductoMasVendido(string Nombre, string Categoria, int Cantidad, decimal Monto);
    public record PedidoReciente(string Id, string Cliente, string Estado, decimal Total);

    public class DashboardStats
    {
        public int TotalProductos { get; set; }
        public int TotalUsuarios { get; set; }
        public int StockTotal { get; set; }
        public decimal InversionTotal { get; set; }
        public decimal VentasHoy { get; set; }
        public int PedidosPendientes { get; set; }
        public int ClientesRegistrados { get; set; }

        public List<VentaPorDia> HistoricoVentas { get; set; } = new();
        public List<VentaPorCategoria> VentasPorCategoria { get; set; } = new();
        public List<ProductoMasVendido> Top10Productos { get; set; } = new();
        public List<PedidoReciente> UltimosPedidos { get; set; } = new();

        public decimal TendenciaVentas { get; set; }
        public decimal TendenciaPedidos { get; set; }
    }

    // ==========================================================
    // 2. EL SERVICIO PRINCIPAL CON SEMÁFORO
    // ==========================================================
    public class DashboardService
    {
        private readonly ApplicationDbContext _context;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public DashboardService(ApplicationDbContext context) => _context = context;

        public async Task<DashboardStats> ObtenerEstadisticasAsync(int diasAtras = 7)
        {
            await _semaphore.WaitAsync();

            try
            {
                var hoy = DateTime.Today;
                var fechaInicio = hoy.AddDays(-(diasAtras - 1));

                var stockTotal = await _context.Productos.SumAsync(p => (int?)p.Stock) ?? 0;
                var productosInv = await _context.Productos.Select(p => new { p.PrecioCompra, p.Stock }).ToListAsync();
                decimal inversionTotal = productosInv.Sum(p => p.PrecioCompra * p.Stock);

                int totalClientes = await _context.Usuarios.CountAsync(u => u.Rol == "Cliente");
                int pendientes = await _context.Pedidos.CountAsync(p => p.Estado == EstadoPedido.Pendiente);
                int pedidosHoy = await _context.Pedidos.CountAsync(p => p.FechaCreacion >= hoy);

                decimal ventasHoy = await _context.Pedidos
                    .Where(p => p.FechaCreacion >= hoy)
                    .SumAsync(p => (decimal?)p.Total) ?? 0m;

                decimal ventasAyer = await _context.Pedidos
                    .Where(p => p.FechaCreacion >= hoy.AddDays(-1) && p.FechaCreacion < hoy)
                    .SumAsync(p => (decimal?)p.Total) ?? 0m;

                var ultimos = await _context.Pedidos
                    .Include(p => p.Cliente)
                    .OrderByDescending(p => p.FechaCreacion)
                    .Take(5)
                    .Select(p => new PedidoReciente(
                        "PED-" + p.Id,
                        p.Cliente != null ? p.Cliente.CorreoElectronico : "Consumidor Final",
                        p.Estado.ToString(),
                        p.Total
                    ))
                    .ToListAsync();

                var ventasRaw = await _context.Pedidos
                    .Where(p => p.FechaCreacion >= fechaInicio)
                    .Select(p => new { p.FechaCreacion.Date, p.Total })
                    .ToListAsync();

                var historico = Enumerable.Range(0, diasAtras)
                    .Select(offset => {
                        var f = fechaInicio.AddDays(offset);
                        var totalDia = ventasRaw.Where(v => v.Date == f.Date).Sum(v => v.Total);
                        return new VentaPorDia(f.ToString("dd MMM"), totalDia, f);
                    }).ToList();

                var detallesRaw = await _context.Set<DetallePedido>()
                    .Include(dp => dp.Producto)
                    .Include(dp => dp.Pedido)
                    .Where(dp => dp.Pedido != null && dp.Pedido.FechaCreacion >= fechaInicio)
                    .Select(dp => new {
                        dp.ProductoId,
                        NombreProducto = dp.Producto.Nombre,
                        CategoriaProducto = dp.Producto.Categoria,
                        dp.Cantidad,
                        MontoLineal = dp.PrecioUnitario * dp.Cantidad
                    })
                    .ToListAsync();

                decimal totalMontoDetalles = detallesRaw.Sum(d => d.MontoLineal);

                var ventasPorCategoria = detallesRaw
                    .GroupBy(d => d.CategoriaProducto ?? "General")
                    .Select(g => new VentaPorCategoria(
                        g.Key,
                        g.Sum(d => d.MontoLineal),
                        totalMontoDetalles > 0 ? (g.Sum(d => d.MontoLineal) / totalMontoDetalles) * 100m : 0m
                    ))
                    .OrderByDescending(v => v.Porcentaje)
                    .ToList();

                var top10Productos = detallesRaw
                    .GroupBy(dp => new { dp.ProductoId, NombreProducto = dp.NombreProducto ?? "Desconocido", CategoriaProducto = dp.CategoriaProducto ?? "General" })
                    .Select(g => new ProductoMasVendido(
                        g.Key.NombreProducto,
                        g.Key.CategoriaProducto,
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
                    TendenciaVentas = ventasAyer > 0 ? ((ventasHoy - ventasAyer) / ventasAyer) * 100m : (ventasHoy > 0 ? 100m : 0m),
                    TendenciaPedidos = pendientes > 0 ? ((decimal)(pedidosHoy - pendientes) / pendientes) * 100m : (pedidosHoy > 0 ? 100m : 0m)
                };
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}