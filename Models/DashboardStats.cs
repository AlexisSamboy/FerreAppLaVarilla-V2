public class DashboardStats
{
    public decimal VentasHoy { get; set; }
    public int PedidosPendientes { get; set; }
    public int ProductosEnStock { get; set; }
    public int ClientesRegistrados { get; set; }

    // Para las gráficas y tablas
    public List<VentaSemanal> VentasSemanales { get; set; } = new();
    public List<PedidoReciente> UltimosPedidos { get; set; } = new();
}

public record VentaSemanal(string Dia, decimal Monto);
public record PedidoReciente(string Id, string Cliente, string Estado, decimal Total);