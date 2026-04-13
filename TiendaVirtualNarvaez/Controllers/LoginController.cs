using Microsoft.AspNetCore.Mvc;
using TiendaVirtualNarvaez.Data;
using TiendaVirtualNarvaez.Models;
using System.Linq;

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
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Correo == correo && u.Clave == clave);
            if (usuario != null)
            {
                HttpContext.Session.SetString("Usuario", usuario.Nombre);
                HttpContext.Session.SetString("Contraseña", usuario.Clave);

                return RedirectToAction("Index", "Producto");
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
