using FerreAppLaVarilla.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FerreAppLaVarilla.UI.Services
{
    public class CarritoService
    {
        // Esta es la clase que representa un artículo dentro del carrito
        public class CartItem
        {
            public required Producto Producto { get; set; }
            public int Cantidad { get; set; }
        }

        // La memoria real donde se guardan los productos
        private List<CartItem> _items = new();

        // Evento opcional por si luego queremos poner un contador en el Navbar
        public event Action? OnChange;

        // 1. Obtener todos los productos del carrito (El que te faltaba)
        public List<CartItem> ObtenerItems()
        {
            return _items;
        }

        // 2. Agregar un producto (Desde la página Productos)
        public void AgregarProducto(Producto producto, int cantidad = 1)
        {
            var itemExistente = _items.FirstOrDefault(i => i.Producto.Id == producto.Id);

            if (itemExistente != null)
            {
                // Si ya existe, le sumamos la cantidad
                itemExistente.Cantidad += cantidad;
            }
            else
            {
                // Si es nuevo, lo agregamos a la lista
                _items.Add(new CartItem { Producto = producto, Cantidad = cantidad });
            }

            NotificarCambio();
        }

        // 3. Actualizar la cantidad usando los botones + y - del carrito
        public void ActualizarCantidad(int productoId, int nuevaCantidad)
        {
            var item = _items.FirstOrDefault(i => i.Producto.Id == productoId);
            if (item != null)
            {
                item.Cantidad = nuevaCantidad;
                NotificarCambio();
            }
        }

        // 4. Eliminar un producto usando el botón de la basurita
        public void EliminarProducto(int productoId)
        {
            var item = _items.FirstOrDefault(i => i.Producto.Id == productoId);
            if (item != null)
            {
                _items.Remove(item);
                NotificarCambio();
            }
        }

        // 5. Vaciar el carrito completo (Al pagar o darle al botón de vaciar)
        public void LimpiarCarrito()
        {
            _items.Clear();
            NotificarCambio();
        }

        // Avisa a la pantalla que algo cambió para que se actualice
        private void NotificarCambio() => OnChange?.Invoke();
    }
}