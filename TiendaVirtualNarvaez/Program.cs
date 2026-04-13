using Microsoft.EntityFrameworkCore;
using TiendaVirtualNarvaez.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSession();

// Configurar el DbContext en los servicios
builder.Services.AddDbContext<TiendaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Agregar los servicios MVC (Controladores y Vistas)
builder.Services.AddControllersWithViews();

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
