using CatalogoApp.Application.Service;
using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
var builder = WebApplication.CreateBuilder(args);

// Agrega servicios MVC
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opciones =>
    {
        opciones.LoginPath = "/Usuario/IniciarSesion"; // Si alguien sin sesión intenta entrar a algo bloqueado, lo manda aquí
    });
// Ruta del archivo JSON
var jsonPath = Path.Combine(builder.Environment.ContentRootPath, "data", "items.json");
// Ruta del archivo JSON
var jsonPathUsers = Path.Combine(builder.Environment.ContentRootPath, "data", "users.json");

// Registrar repositorio y servicio en la Inyección de Dependencias
builder.Services.AddSingleton<IItemRepository>(new JsonItemRepository(jsonPath));
builder.Services.AddScoped<ItemService>();
builder.Services.AddSingleton<IUserRepository>(new JsonUserRepository(jsonPathUsers));
builder.Services.AddScoped<UserService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Manejo de archivos estáticos (wwwroot)
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); // <-- ¡Esta es la que lee el gafete!
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();