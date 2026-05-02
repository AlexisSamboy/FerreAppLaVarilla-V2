using FerreAppLaVarilla.UI.Components;
// ⬇️ Estos son tus namespaces correctos
using FerreAppLaVarilla.UI.Services;
using Microsoft.AspNetCore.Authentication;
//using Microsoft.EntityFrameworkCore;
// using FerreAppLaVarilla.Data; // Descomenta esta línea si tu proyecto de datos se llama así

var builder = WebApplication.CreateBuilder(args);

// ====================================================================
// 1. SERVICIOS NATIVOS DE BLAZOR
// ====================================================================
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ====================================================================
// 2. CONEXIÓN A SQL SERVER (Solo si ya creaste el DbContext)
// ====================================================================
/*
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=FerreAppLaVarilla;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"));
*/

// ====================================================================
// 3. SERVICIOS DEL CARRITO Y AUTENTICACIÓN
// ====================================================================
builder.Services.AddScoped<CarritoService>();
builder.Services.AddScoped<AutenticacionService>();

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