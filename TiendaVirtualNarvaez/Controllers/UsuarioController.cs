using Microsoft.AspNetCore.Mvc;
using TiendaVirtualNarvaez.Data;
using TiendaVirtualNarvaez.Models;
using Microsoft.EntityFrameworkCore;
using TiendaVirtualNarvaez.Helpers;

namespace TiendaVirtualNarvaez.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly TiendaContext _context;

        public UsuarioController(TiendaContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetString("Usuario") == null)
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);
            }
            base.OnActionExecuting(context);
        }

        // LISTAR USUARIOS (CON FILTRO DE ROL)
        public IActionResult Index()
        {
            // 1. Obtener datos de la sesión
            string correoSesion = HttpContext.Session.GetString("Usuario");
            string rolSesion = HttpContext.Session.GetString("Rol");

            // Seguridad básica: si no hay sesión, al login
            if (string.IsNullOrEmpty(correoSesion))
            {
                return RedirectToAction("Index", "Login");
            }

            if (rolSesion == "administrador")
            {
                var usuarios = _context.Usuarios.ToList();
                return View(usuarios);
            }
            else
            {
                // CORRECCIÓN: Filtramos asegurando que no afecten espacios o mayúsculas
                var usuarios = _context.Usuarios
                    .Where(u => u.Correo.ToLower().Trim() == correoSesion.ToLower().Trim())
                    .ToList();

                return View(usuarios);
            }
        }

        // DETALLES DEL USUARIO (CON PROTECCIÓN)
        public IActionResult Details(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            string correoSesion = HttpContext.Session.GetString("Usuario");
            string rolSesion = HttpContext.Session.GetString("Rol");

            if (usuario == null) return NotFound();

            // Bloqueo: Si no es admin y el detalle no es el suyo
            if (rolSesion != "administrador" && usuario.Correo != correoSesion)
            {
                return RedirectToAction("Index");
            }

            return View(usuario);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Usuario usuario)
        {   //Convertir contraña a hash 
            usuario.Clave = HashHelper.ObtenerHash(usuario.Clave);

            if (!ModelState.IsValid) return View(usuario);
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // FORMULARIO EDITAR (CON PROTECCIÓN)
        public IActionResult Edit(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            string correoSesion = HttpContext.Session.GetString("Usuario");
            string rolSesion = HttpContext.Session.GetString("Rol");

            if (usuario == null) return NotFound();

            // Seguridad: Un usuario normal no puede editar a otros aunque sepa el ID
            if (rolSesion != "administrador" && usuario.Correo != correoSesion)
            {
                return RedirectToAction("Index");
            }

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Usuario usuario)
        {
            if (!ModelState.IsValid) return View(usuario);

            _context.Usuarios.Update(usuario);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // ELIMINAR USUARIO (SOLO ADMIN)
        public IActionResult Delete(int id)
        {
            string rolSesion = HttpContext.Session.GetString("Rol");

            // Solo el admin puede eliminar
            if (rolSesion != "administrador")
            {
                return RedirectToAction("Index");
            }

            var usuario = _context.Usuarios.Find(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}