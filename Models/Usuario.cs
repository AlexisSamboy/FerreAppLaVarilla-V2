using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FerreAppLaVarilla.UI.Models
{
    [Table("Usuarios")] // Asegura que EF busque la tabla correcta en SQL
    public class Usuario
    {
        [Key] // Define que este es el ID primario
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        public string CorreoElectronico { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Rol { get; set; } = "Cliente"; // Valor por defecto

        // --- CAMPOS ADICIONALES (Vistos en tu captura de SQL) ---

        public string? Telefono { get; set; }

        public string? CedulaRNC { get; set; }

        public string? DireccionEntrega { get; set; }

        // --- PROPIEDADES DE NAVEGACIÓN (Relaciones) ---

        // Esto permite que desde un Usuario puedas ver todos sus pedidos
        public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}