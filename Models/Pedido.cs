using System.ComponentModel.DataAnnotations;

namespace FerreAppLaVarilla.UI.Models
{
    public enum EstadoPedido
    {
        Pendiente,
        EnRuta,
        Entregado,
        Completado,
        Cancelado
    }

    public class Pedido
    {
        public int Id { get; set; }

        public DateTime FechaCreacion { get; set; }

        public EstadoPedido Estado { get; set; }

        // =========================
        // CLIENTE
        // =========================
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        // =========================
        // CAMIÓN
        // =========================
        public int? CamionAsignadoId { get; set; }
        public Camion? CamionAsignado { get; set; }

        public bool RequiereDelivery { get; set; }

        public string TipoDocumento { get; set; } = "Factura";

        // =========================
        // DETALLES
        // =========================
        public List<DetallePedido> Articulos { get; set; } = new();
    }
}