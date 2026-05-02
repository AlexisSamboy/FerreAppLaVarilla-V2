namespace FerreAppLaVarilla.UI.Services
{
    public class AutenticacionService
    {
        public dynamic? UsuarioActual { get; private set; }
        public bool IsAutenticado => UsuarioActual != null;

        public void IniciarSesion(dynamic usuario) => UsuarioActual = usuario;
        public void CerrarSesion() => UsuarioActual = null;
    }
}