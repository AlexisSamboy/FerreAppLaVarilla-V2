namespace FerreAppLaVarilla.UI.Models
{
    public class DetallePedido
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }

        public int ProductoId { get; set; } // Agrega esto para saber qué compró

        public Producto? Producto { get; set; } // ¡Sin la palabra required!

        public int Cantidad { get; set; }
        public int PrecioUnitario { get; internal set; }
    }
}