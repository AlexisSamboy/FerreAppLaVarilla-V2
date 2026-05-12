using FerreAppLaVarilla.UI.Components;
using FerreAppLaVarilla.UI.Data;
using FerreAppLaVarilla.UI.Services;
using Microsoft.AspNetCore.Authentication.Cookies; // Añadido para la autenticación
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ====================================================================
// 1. SERVICIOS NATIVOS DE BLAZOR
// ====================================================================
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ====================================================================
// 1.1 AUTENTICACIÓN Y AUTORIZACIÓN (¡LA SOLUCIÓN AL ERROR!)
// ====================================================================
// A. Registramos los servicios nativos que el servidor te estaba pidiendo
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login"; // Ajusta esto si tu ruta es diferente
    });

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState(); // Vital para que los componentes <AuthorizeView> funcionen bien

// B. Registramos tus servicios personalizados (Eliminé los duplicados)
builder.Services.AddScoped<AutenticacionService>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<AutenticacionService>());

// ====================================================================
// 2. CONEXIÓN A SQL SERVER
// ====================================================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=FerreAppLaVarilla;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"));

// ====================================================================
// 3. TUS SERVICIOS DEL NEGOCIO
// ====================================================================
builder.Services.AddScoped<CarritoService>();
builder.Services.AddScoped<ProductoService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<FerreAppLaVarilla.UI.Models.ServicioDespacho>(); // Nota: Generalmente no se inyectan "Models", pero lo dejo intacto si tu lógica lo requiere.
builder.Services.AddScoped<PdfService>();
builder.Services.AddSingleton<PedidoNotificationService>();

var app = builder.Build();

// ====================================================================
// CONFIGURACIÓN DEL PIPELINE DE PETICIONES HTTP
// ====================================================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();

// ¡ESTO ES CRÍTICO! El orden debe ser exactamente este:
app.UseAuthentication(); // 1. Verifica la identidad del usuario
app.UseAuthorization();  // 2. Verifica los permisos del usuario

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();