namespace FerreAppLaVarilla.UI.Models
{
    public class ServicioFacturacion
    {
        private const decimal ITBIS = 0.18m;

        // Método principal de facturación
        public decimal CalcularTotal(List<DetallePedido> detalles)
        {
            decimal subtotal = detalles.Sum(d => d.PrecioUnitario * d.Cantidad);

            decimal descuento = CalcularDescuento(subtotal);

            decimal subtotalConDescuento = subtotal - descuento;

            decimal impuesto = subtotalConDescuento * ITBIS;

            decimal totalFinal = subtotalConDescuento + impuesto;

            return totalFinal;
        }

        // Descuento por compras mayores a RD$10,000
        public decimal CalcularDescuento(decimal subtotal)
        {
            if (subtotal > 10000)
            {
                return subtotal * 0.05m; // 5% descuento
            }

            return 0;
        }

        // Calcular ITBIS
        public decimal CalcularITBIS(decimal monto)
        {
            return monto * ITBIS;
        }

        // Verificar disponibilidad en almacén
        public bool HayStock(Producto producto, int cantidad)
        {
            return producto.Stock >= cantidad;
        }

        // Reducir stock después de venta
        public void ReducirStock(Producto producto, int cantidad)
        {
            if (HayStock(producto, cantidad))
            {
                producto.Stock -= cantidad;
            }
            else
            {
                throw new Exception("No hay suficiente stock.");
            }
        }

        // Calcular peso total de materiales pesados
        public double CalcularPesoCarga(List<DetallePedido> detalles)
        {
            double pesoTotal = 0;

            foreach (var item in detalles)
            {
                if (item.Producto is MaterialPesado material)
                {
                    pesoTotal += material.PesoUnidad * item.Cantidad;
                }
            }

            return pesoTotal;
        }
    }
}
