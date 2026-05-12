using System;

namespace FerreAppLaVarilla.UI.Services
{
    public class PedidoNotificationService
    {
        // Este es el evento o "campanita" que va a sonar
        public event Action<string>? OnNuevoPedido;

        // Método que el Cliente llamará para hacer sonar la campana
        public void NotificarNuevoPedido(string mensaje)
        {
            OnNuevoPedido?.Invoke(mensaje);
        }
    }
}