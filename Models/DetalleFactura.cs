namespace FerreAppLaVarilla.UI.Models
{
    public class DetalleFactura
    {
        public string Producto { get; set; } = "";

        public int Cantidad { get; set; }

        public decimal Precio { get; set; }

        public decimal Subtotal { get; set; }
    }
}