using System;
using System.Collections.Generic;

namespace FerreAppLaVarilla.UI.Models
{
    // 1. Aquí definimos exactamente cuáles son las opciones de Estado (Esto arregla el error de "Pendiente")
    public enum EstadoPedido
    {
        Pendiente = 0,
        EnRuta = 1,
        Completado = 2,
        Cancelado = 3
    }

    public class Pedido
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; }

        public required Cliente Cliente { get; set; }

        public EstadoPedido Estado { get; set; }

        // 2. ¡ELIMINAMOS el 'required'! Ahora el pedido puede nacer tranquilo sin exigir un camión
        public Camion? CamionAsignado { get; set; }

        public List<DetallePedido> Articulos { get; set; } = new List<DetallePedido>();
    }
}