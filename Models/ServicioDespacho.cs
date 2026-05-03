using System;
using System.Collections.Generic;

namespace FerreAppLaVarilla.UI.Models
{
    public class ServicioDespacho
    {
        // 1. VALIDACIÓN DE INVENTARIO
        public bool ValidarStock(Producto producto, int cantidadSolicitada)
        {
            return producto.Stock >= cantidadSolicitada;
        }

        // 2. CÁLCULO DE PESO DEL PEDIDO
        public double CalcularPesoTotal(List<DetallePedido> detalles)
        {
            double pesoTotal = 0;

            foreach (var item in detalles)
            {
                // Verificamos si el producto es un material pesado (por herencia)
                if (item.Producto is MaterialPesado material)
                {
                    // CORRECCIÓN APLICADA: Ahora usamos PesoUnidad
                    pesoTotal += material.PesoUnidad * item.Cantidad;
                }
            }

            return pesoTotal;
        }

        // 3. VALIDACIÓN DE CAPACIDAD DEL CAMIÓN
        public bool ValidarCapacidadCamion(double pesoTotalPedido, Camion camion)
        {
            return pesoTotalPedido <= camion.CapacidadMaxima;
        }

        // 4. CONTROL DE DESPACHO Y CAMBIO DE ESTADOS
        public void ProcesarDespacho(Pedido pedido, Camion camion)
        {
            // 1. Calcular el peso total del pedido
            double pesoTotal = CalcularPesoTotal(pedido.Articulos);

            // 2. Validar si el camión soporta el peso
            if (!ValidarCapacidadCamion(pesoTotal, camion))
            {
                throw new Exception("El camión no tiene capacidad suficiente.");
            }

            // 3. Cambiar estado del pedido
            pedido.Estado = EstadoPedido.EnRuta;

            // 4. Cambiar estado del camión
            camion.Disponible = false;
        }
    }
}