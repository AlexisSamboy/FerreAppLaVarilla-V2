using FerreAppLaVarilla.UI.Models;

namespace FerreAppLaVarilla.UI.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; }

        public required Cliente Cliente { get; set; }

        public EstadoPedido Estado { get; set; }

        public required Camion? CamionAsignado { get; set; }

        public List<DetallePedido> Articulos { get; set; }
            = new List<DetallePedido>();
    }
}