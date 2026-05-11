using FerreAppLaVarilla.UI.Models;

public class Cotizacion
{
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    public string ClienteNombre { get; set; } = "";

    public string ClienteTelefono { get; set; } = "";

    public string ClienteDireccion { get; set; } = "";

    public decimal Subtotal { get; set; }

    public decimal Itbis { get; set; }

    public decimal Total { get; set; }

    public string Estado { get; set; } = "Pendiente";

    public List<DetalleCotizacion> Detalles { get; set; } = new();
}
public class DetalleCotizacion
{
    public int Id { get; set; }

    public int CotizacionId { get; set; }

    public Cotizacion Cotizacion { get; set; }

    public int ProductoId { get; set; }

    public Producto Producto { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }
}