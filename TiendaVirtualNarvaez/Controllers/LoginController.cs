using Microsoft.AspNetCore.Mvc;
using TiendaVirtualNarvaez.Data;
using TiendaVirtualNarvaez.Models;
using System.Linq;
using TiendaVirtualNarvaez.Helpers;

namespace TiendaVirtualJojoa.Controllers
{
    public class LoginController : Controller
    {
        private readonly TiendaContext _context;
        public LoginController(TiendaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string correo, string clave)
        {   
            string claveHash = HashHelper.ObtenerHash(clave);

            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Correo == correo && u.Clave == claveHash);

            if (usuario != null)
            {
                // CAMBIO CRÍTICO: Guarda el Correo, no el Nombre, en la sesión "Usuario"
                HttpContext.Session.SetString("Usuario", usuario.Correo);

                HttpContext.Session.SetString("NombreUsuario", usuario.Nombre); // Opcional: para mostrar el nombre en el Layout
                HttpContext.Session.SetString("Rol", usuario.Rol);

                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = "Credenciales incorrectas";
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
