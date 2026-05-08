namespace FerreAppLaVarilla.UI.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public required string NombreCompleto { get; set; }
        public required string CedulaRNC { get; set; }
        public required string DireccionEntrega { get; set; }
        public required string Telefono { get; set; }

        public static implicit operator string(Cliente v)
        {
            throw new NotImplementedException();
        }
    }
}