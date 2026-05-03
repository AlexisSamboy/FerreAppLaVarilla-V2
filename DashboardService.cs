using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FerreAppLaVarilla.UI.Data;

namespace FerreAppLaVarilla.UI.Services
{
    public class DashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardStats> ObtenerEstadisticasAsync()
        {
            // Protegemos el cálculo de la inversión separando la matemática
            var stockTotal = await _context.Productos.SumAsync(p => (int?)p.Stock) ?? 0;

            // Calculamos la inversión trayendo los datos necesarios para evitar conflictos en SQL
            var productos = await _context.Productos.Select(p => new { p.PrecioCompra, p.Stock }).ToListAsync();
            decimal inversionTotal = productos.Sum(p => p.PrecioCompra * p.Stock);

            return new DashboardStats
            {
                TotalProductos = await _context.Productos.CountAsync(),
                TotalUsuarios = await _context.Usuarios.CountAsync(),
                StockTotal = stockTotal,
                InversionTotal = inversionTotal
            };
        }
    }

    public class DashboardStats
    {
        public int TotalProductos { get; set; }
        public int TotalUsuarios { get; set; }
        public int StockTotal { get; set; }
        public decimal InversionTotal { get; set; }
    }
}