using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TiendaVirtualNarvaez.Data;
using TiendaVirtualNarvaez.Models;
using static System.Net.Mime.MediaTypeNames;

namespace TiendaVirtualNarvaez.Controllers
{
    public class ProductoController : Controller
    {
        private readonly TiendaContext _context;

        public ProductoController(TiendaContext context)
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

        // DETALLES DEL PRODUCTO
        public IActionResult Details(int id)
        {
            // Buscamos el producto e incluimos su categoría
            var producto = _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefault(p => p.Id == id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // FORMULARIO CREAR
        public IActionResult Create()
        {
            ViewBag.Categorias = ObtenerListaConIds(); 
            return View();
        }

        // GUARDAR PRODUCTO
        [HttpPost]
        public IActionResult Create(Producto producto, IFormFile imagen)
        {
            var existeCategoria = _context.Categorias
                .Any(c => c.Id == producto.CategoriaId);

            if (!existeCategoria)
            {
                ModelState.AddModelError("CategoriaId", "La categoría no existe");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = ObtenerListaConIds(); 
                return View(producto);
            }

            if (imagen != null)
            {
                var ruta = Path.Combine(Directory.GetCurrentDirectory(),
                                         "wwwroot/images",
                                         imagen.FileName);

                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    imagen.CopyTo(stream);
                }

                producto.ImagenUrl = "/images/" + imagen.FileName;
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
        // GET: Mostrar formulario de edición
        public IActionResult Edit(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null)
                return NotFound();

            ViewBag.Categorias = ObtenerListaConIds();
            return View(producto);
        }

        // POST: Guardar cambios del producto
        [HttpPost]
        public IActionResult Edit(Producto producto, IFormFile? imagen)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = ObtenerListaConIds();
                return View(producto);
            }

            var productoBD = _context.Productos.Find(producto.Id);
            if (productoBD == null)
                return NotFound();

            productoBD.Nombre = producto.Nombre;
            productoBD.Precio = producto.Precio;
            productoBD.Stock = producto.Stock;
            productoBD.CategoriaId = producto.CategoriaId;

            if (imagen != null)
            {
                var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                var ruta = Path.Combine(carpeta, imagen.FileName);
                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    imagen.CopyTo(stream);
                }

                productoBD.ImagenUrl = "/images/" + imagen.FileName;
            }

            _context.Productos.Update(productoBD);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // ELIMINAR PRODUCTO
        public IActionResult Delete(int id)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "administrador")
            {
                return RedirectToAction("Index", "Login");
            }

            var producto = _context.Productos.Find(id);
            if (producto == null) return RedirectToAction("Index");

            int categoriaId = producto.CategoriaId;

            _context.Productos.Remove(producto);

            var categoria = _context.Categorias.Find(categoriaId);
            if (categoria != null)
            {
                bool tieneProductos = _context.Productos.Any(p => p.CategoriaId == categoriaId && p.Id != id);
                categoria.Estado = tieneProductos ? "Activo" : "Inactivo";
                _context.Categorias.Update(categoria);
            }

            _context.SaveChanges();
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