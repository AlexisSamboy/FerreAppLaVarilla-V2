using Microsoft.EntityFrameworkCore;
using FerreAppLaVarilla.UI.Models; // Asegúrate de que este using apunte a tus clases

namespace FerreAppLaVarilla.UI.Data
{
    // Heredamos de DbContext para que esta clase sea el "puente" a SQL
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Estas serán tus tablas reales en SQL Server
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallesPedido { get; set; }
        public DbSet<Camion> Camiones { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}