using System.ComponentModel.DataAnnotations.Schema;

namespace FerreAppLaVarilla.UI.Models
{
    public class Producto
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        public string Categoria { get; set; } = string.Empty;

        public string Marca { get; set; } = string.Empty;

        public string UnidadMedida { get; set; } = string.Empty;

        public string ImagenUrl { get; set; } = string.Empty;

        public string Sku { get; set; } = string.Empty;

        public decimal Precio { get; set; }

        public decimal PrecioCompra { get; set; }

        public int Stock { get; set; }

        public int StockMinimo { get; set; }

        public bool Activo { get; set; }

        // =========================
        // SOLO PARA EL POS
        // NO SE GUARDA EN SQL
        // =========================

        [NotMapped]
        public int CantidadTemporal { get; set; } = 1;
    }
}