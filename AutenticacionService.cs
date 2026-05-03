using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using FerreAppLaVarilla.UI.Models;

namespace FerreAppLaVarilla.UI.Services
{
    public class AutenticacionService : AuthenticationStateProvider
    {
        private ClaimsPrincipal _usuarioActual = new ClaimsPrincipal(new ClaimsIdentity());

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(_usuarioActual));
        }

        public void MarcarUsuarioComoAutenticado(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Correo),
                new Claim(ClaimTypes.Role, usuario.Rol)
            };

            var identity = new ClaimsIdentity(claims, "FerreAppAuth");
            _usuarioActual = new ClaimsPrincipal(identity);

            // ¡CRUCIAL! Notificamos usando Task.FromResult para evitar desincronizaciones
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_usuarioActual)));
        }

        public void CerrarSesion()
        {
            _usuarioActual = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_usuarioActual)));
        }
    }
}