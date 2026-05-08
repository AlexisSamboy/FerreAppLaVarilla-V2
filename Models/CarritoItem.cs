namespace FerreAppLaVarilla.UI.Models
{
    public class CarritoItem
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public string? ImagenUrl { get; set; }

        // Propiedad calculada para el subtotal
        public decimal Subtotal => Precio * Cantidad;
    }
}