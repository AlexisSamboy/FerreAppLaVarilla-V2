    namespace FerreAppLaVarilla.UI.Models
    {
        public class Producto
        {
        public int Id { get; set; }

        // Información Básica
        public string Nombre { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        // Precios e Inventario
        public decimal PrecioCompra { get; set; }
        public decimal Precio { get; set; } // Este es el precio de venta
        public int Stock { get; set; }
        public int StockMinimo { get; set; }
        public string UnidadMedida { get; set; } = "Unidad";
        public bool Activo { get; set; } = true;

        // Multimedia
        public string ImagenUrl { get; set; } = "default.png";
    }
    }