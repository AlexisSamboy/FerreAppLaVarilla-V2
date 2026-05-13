using System.ComponentModel.DataAnnotations;

namespace FerreAppLaVarilla.UI.Models
{
    public class DetallePedido
    {
        public int Id { get; set; }

        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        public int Cantidad { get; set; }

        // Nombre exacto como aparece en tu SQL Management Studio
        public decimal PrecioUnitario { get; set; }

        public int? PedidoId { get; set; }

        // ============================================================
        // EL PUENTE HACIA LA FECHA:
        // Gracias a esta propiedad, en tus reportes puedes hacer:
        // detalle.Pedido.FechaCreacion
        // ============================================================
        public Pedido? Pedido { get; set; }
    }
}