using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FerreAppLaVarilla.UI.Models
{
    public enum EstadoPedido
    {
        Pendiente,
        EnRuta,
        Entregado,
        Completado,
        Cancelado
    }

    public class Pedido
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Required]
        public EstadoPedido Estado { get; set; } = EstadoPedido.Pendiente;

        // =========================
        // MONTO TOTAL (PARA REPORTES)
        // =========================
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        // =========================
        // CLIENTE
        // =========================
        [Required]
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        // =========================
        // CAMIÓN / LOGÍSTICA
        // =========================
        public int? CamionAsignadoId { get; set; }
        public Camion? CamionAsignado { get; set; }

        public bool RequiereDelivery { get; set; }

        public string TipoDocumento { get; set; } = "Factura";

        // =========================
        // DETALLES DEL PEDIDO
        // =========================
        public List<DetallePedido> Articulos { get; set; } = new();
    }
}