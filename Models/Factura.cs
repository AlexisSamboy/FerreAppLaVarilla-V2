using System.ComponentModel.DataAnnotations;

namespace FerreAppLaVarilla.UI.Models
{
    public class Factura
    {
        public string NumeroFactura { get; set; } = "";

        public string Cliente { get; set; } = "";

        public string CedulaRnc { get; set; } = "";

        public DateTime Fecha { get; set; } = DateTime.Now;

        public decimal Subtotal { get; set; }

        public decimal Itbis { get; set; }

        public decimal Total
        {
            get
            {
                return Subtotal + Itbis;
            }
        }

        public List<DetalleFactura> Detalles { get; set; } = new();
    }
}