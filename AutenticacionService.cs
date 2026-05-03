using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using FerreAppLaVarilla.UI.Models;

namespace FerreAppLaVarilla.UI.Services
{
    public class AutenticacionService : AuthenticationStateProvider
    {
        private ClaimsPrincipal _usuarioActual =
            new ClaimsPrincipal(new ClaimsIdentity());

        // Propiedad para mostrar el nombre del usuario autenticado
        public string NombreUsuario =>
            _usuarioActual.Identity?.Name ?? "Invitado";

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(
                new AuthenticationState(_usuarioActual)
            );
        }

        public void MarcarUsuarioComoAutenticado(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                // IMPORTANTE:
                // Aquí usamos Correo porque en tu modelo Usuario
                // probablemente no existe la propiedad Nombre
                new Claim(ClaimTypes.Name, usuario.Correo),

                // Si Rol puede venir null, protegemos con ?? ""
                new Claim(ClaimTypes.Role, usuario.Rol ?? "")
            };

            var identity = new ClaimsIdentity(
                claims,
                "FerreAppAuth"
            );

            _usuarioActual = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(
                Task.FromResult(
                    new AuthenticationState(_usuarioActual)
                )
            );
        }

        public void CerrarSesion()
        {
            _usuarioActual =
                new ClaimsPrincipal(new ClaimsIdentity());

            NotifyAuthenticationStateChanged(
                Task.FromResult(
                    new AuthenticationState(_usuarioActual)
                )
            );
        }
    }
}