using FerreAppLaVarilla.UI.Components;
using FerreAppLaVarilla.UI.Data;

// ⬇️ Estos son tus namespaces correctos
using FerreAppLaVarilla.UI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ====================================================================
// 1. SERVICIOS NATIVOS DE BLAZOR
// ====================================================================
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ====================================================================
// 1.1 AUTENTICACIÓN
// ====================================================================

// 1. Registramos la clase concreta
builder.Services.AddScoped<FerreAppLaVarilla.UI.Services.AutenticacionService>();

// 2. Le decimos a Blazor que use esa misma clase para la seguridad del sistema
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<FerreAppLaVarilla.UI.Services.AutenticacionService>());

// ====================================================================
// 2. CONEXIÓN A SQL SERVER
// ====================================================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=FerreAppLaVarilla;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"));

// ====================================================================
// 3. SERVICIOS DEL CARRITO Y AUTENTICACIÓN
// ====================================================================
builder.Services.AddScoped<CarritoService>();
builder.Services.AddScoped<AutenticacionService>();
builder.Services.AddScoped<FerreAppLaVarilla.UI.Services.ProductoService>();
builder.Services.AddScoped<FerreAppLaVarilla.UI.Services.UsuarioService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<FerreAppLaVarilla.UI.Models.ServicioDespacho>();
builder.Services.AddScoped<FerreAppLaVarilla.UI.Services.CarritoService>();
builder.Services.AddScoped<PdfService>();

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

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();