using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaVirtualNarvaez.Data;
using TiendaVirtualNarvaez.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TiendaVirtualNarvaez.Controllers
{
    public class ProductoController : Controller
    {
        private readonly TiendaContext _context;

        public ProductoController(TiendaContext context)
        {
            _context = context;
        }

        // LISTAR PRODUCTOS
        public IActionResult Index()
        {   
            if(HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var productos = _context.Productos
                .Include(p => p.Categoria)
                .ToList();

            return View(productos);
        }

        // FORMULARIO CREAR
        public IActionResult Create()
        {
            ViewBag.Categorias = ObtenerListaConIds(); // 🔥 CORREGIDO
            return View();
        }

        // GUARDAR PRODUCTO
        [HttpPost]
        public IActionResult Create(Producto producto)
        {
            var existeCategoria = _context.Categorias
                .Any(c => c.Id == producto.CategoriaId);

            if (!existeCategoria)
            {
                ModelState.AddModelError("CategoriaId", "La categoría no existe");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = ObtenerListaConIds(); // 🔥 IMPORTANTE
                return View(producto);
            }

            _context.Productos.Add(producto);
            _context.SaveChanges();

            var categoria = _context.Categorias.Find(producto.CategoriaId);

            if (categoria != null)
            {
                categoria.Estado = "Activo";
                _context.Categorias.Update(categoria);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // FORMULARIO EDITAR
        public IActionResult Edit(int id)
        {
            var producto = _context.Productos.Find(id);
            ViewBag.Categorias = ObtenerListaConIds();
            return View(producto);
        }

        // ACTUALIZAR PRODUCTO
        [HttpPost]
        public IActionResult Edit(Producto producto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = ObtenerListaConIds();
                return View(producto);
            }

            _context.Productos.Update(producto);
            _context.SaveChanges();

            var categoriaNueva = _context.Categorias.Find(producto.CategoriaId);

            if (categoriaNueva != null)
            {
                categoriaNueva.Estado = "Activo";
                _context.Categorias.Update(categoriaNueva);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // ELIMINAR PRODUCTO
        public IActionResult Delete(int id)
        {
            var producto = _context.Productos.Find(id);

            if (producto != null)
            {
                int categoriaId = producto.CategoriaId;

                _context.Productos.Remove(producto);
                _context.SaveChanges();

                bool tieneProductos = _context.Productos
                    .Any(p => p.CategoriaId == categoriaId);

                var categoria = _context.Categorias.Find(categoriaId);

                if (categoria != null)
                {
                    categoria.Estado = tieneProductos ? "Activo" : "Inactivo";
                    _context.Categorias.Update(categoria);
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        // LISTA PARA SELECT (YA ESTABA BIEN)
        private SelectList ObtenerListaConIds()
        {
            var lista = _context.Categorias
                .Select(c => new {
                    Id = c.Id,
                    Texto = c.Id + " - " + c.Nombre
                }).ToList();

            return new SelectList(lista, "Id", "Texto");
        }
    }
}