using Microsoft.EntityFrameworkCore;
using FerreAppLaVarilla.UI.Data;
using FerreAppLaVarilla.UI.Models;

namespace FerreAppLaVarilla.UI.Services
{
    public class ProductoService
    {
        private readonly ApplicationDbContext _context;

        // Aquí inyectamos la base de datos
        public ProductoService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método para buscar todos los productos
        public async Task<List<Producto>> ObtenerProductosAsync()
        {
            return await _context.Productos.ToListAsync();
        }
    }
}