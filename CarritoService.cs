using FerreAppLaVarilla.UI.Models;

namespace FerreAppLaVarilla.UI.Services
{
    public class CarritoService
    {
        public List<CarritoItem> Items { get; private set; } = new();

        public void AgregarProducto(Producto producto, int cantidad)
        {
            var itemExistente = Items.FirstOrDefault(i => i.ProductoId == producto.Id);

            if (itemExistente != null)
            {
                itemExistente.Cantidad += cantidad;
            }
            else
            {
                Items.Add(new CarritoItem
                {
                    ProductoId = producto.Id,
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    Cantidad = cantidad,
                    ImagenUrl = producto.ImagenUrl
                });
            }
        }

        public void QuitarProducto(int productoId) => Items.RemoveAll(i => i.ProductoId == productoId);

        public decimal ObtenerTotal() => Items.Sum(i => i.Subtotal);

        public void LimpiarCarrito() => Items.Clear();
    }
}