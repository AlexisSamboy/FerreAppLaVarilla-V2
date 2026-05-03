    namespace FerreAppLaVarilla.UI.Models
    {
        public class Producto
        {
            public int Id { get; set; } 
            public required string Nombre { get; set; }
            public required string Descripcion { get; set; }
            public decimal Precio { get; set; }
            public int Stock { get; set; }

        // Guardará el nombre del archivo de la foto (ej. "taladro.png")
            public string ImagenUrl { get; set; } = "default.png";
        }
    }