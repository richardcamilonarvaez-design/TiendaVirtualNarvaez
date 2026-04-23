using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaVirtualNarvaez.Data;
using TiendaVirtualNarvaez.Models;
using TiendaVirtualNarvaez.Data;
using TiendaVirtualNarvaez.Models;

namespace TiendaVirtualNarvaez.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly TiendaContext _context; // Contexto de base de datos

        // Inyección de dependencia para acceder a la base de datos
        public CategoriaController(TiendaContext context)
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

        // 1. LISTAR CATEGORIAS
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            // Trae todas las categorías de la base de datos
            var categorias = _context.Categorias.ToList();
            return View(categorias);
        }

        // DETALLES DE LA CATEGORIA
        public IActionResult Details(int id)
        {
            var categoria = _context.Categorias.Find(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // 2. FORMULARIO CREAR (GET)
        public IActionResult Create()
        {
            return View();
        }

        // 3. GUARDAR CATEGORIA (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                // Verificar si la categoría tiene productos asociados
                bool tieneProductos = _context.Productos.Any(p => p.CategoriaId == categoria.Id);

                if (!tieneProductos)
                {
                    categoria.Estado = "Inactivo";
                }
                else
                {
                    categoria.Estado = "Activo"; // Cambiar estado a activo si ya tiene productos
                }

                _context.Categorias.Add(categoria);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // 4. FORMULARIO EDITAR (GET)
        public IActionResult Edit(int id)
        {
            var categoria = _context.Categorias.Find(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // 5. ACTUALIZAR CATEGORIA (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Categorias.Update(categoria);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // 6. ELIMINAR CATEGORIA
        public IActionResult Delete(int id)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "administrador")
            {
                return RedirectToAction("Index", "Login");
            }
            var categoria = _context.Categorias.Find(id);
            if (categoria != null)
            {
                _context.Categorias.Remove(categoria);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
