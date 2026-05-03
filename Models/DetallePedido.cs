namespace FerreAppLaVarilla.UI.Models
{
    public class DetallePedido
    {
        public int Id { get; set; }
        public required Producto Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}