namespace FerreAppLaVarilla.UI.Models
{
    public class MaterialPesado : Producto
    {
        public double PesoUnidad { get; set; }
        public required string TipoCarga { get; set; }
    }
}   