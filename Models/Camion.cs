namespace FerreAppLaVarilla.UI.Models
{
    public class Camion
    {
        public int Id { get; set; }
        public required string Placa { get; set; }
        public required string Descripcion { get; set; }
        public double CapacidadMaxima { get; set; }
        public bool Disponible { get; set; }
    }
}