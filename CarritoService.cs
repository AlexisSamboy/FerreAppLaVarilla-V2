namespace FerreAppLaVarilla.UI.Services
{
    public class CarritoService
    {
        public List<dynamic> Items { get; private set; } = new();

        public void AgregarAlCarrito(dynamic producto, int cantidad)
        {
            Items.Add(new { Producto = producto, Cantidad = cantidad });
        }

        public void LimpiarCarrito() => Items.Clear();
    }
}