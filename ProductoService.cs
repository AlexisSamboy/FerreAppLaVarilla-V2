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

        // Método para buscar un producto por su ID
        public async Task AgregarProductoAsync(Producto nuevoProducto)
        {
            _context.Productos.Add(nuevoProducto);
            await _context.SaveChangesAsync();
        }
        // Busca UN solo producto por su ID
        public async Task<Producto> ObtenerProductoPorIdAsync(int id)
        {
            return await _context.Productos.FindAsync(id);
        }

        // Actualiza el producto en la base de datos
        public async Task ActualizarProductoAsync(Producto productoActualizado)
        {
            _context.Productos.Update(productoActualizado);
            await _context.SaveChangesAsync();
        }

        // Método para eliminar un producto
        public async Task EliminarProductoAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }
        }
    }
}