namespace FerreAppLaVarilla.UI.Models
{
    public class Camion
    {
        public int Id { get; set; }

        public string Placa { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        public double CapacidadMaxima { get; set; }

        public bool Disponible { get; set; } = true;
    }
}