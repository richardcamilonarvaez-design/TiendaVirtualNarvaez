using Microsoft.EntityFrameworkCore;
using TiendaVirtualNarvaez.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSession();

// Configurar el DbContext en los servicios
builder.Services.AddDbContext<TiendaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Agregar los servicios MVC (Controladores y Vistas)
builder.Services.AddControllersWithViews();

// ... código anterior ...

builder.Services.AddControllersWithViews();

// AÑADE ESTA LÍNEA AQUÍ:
builder.Services.AddHttpContextAccessor();

// También asegúrate de tener configurada la sesión si no lo habías hecho:
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configurar el pipeline de solicitud HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
