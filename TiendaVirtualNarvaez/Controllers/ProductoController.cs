using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaVirtualNarvaez.Data;
using TiendaVirtualNarvaez.Models;
using Microsoft.AspNetCore.Mvc.Rendering; // Necesario para SelectList

namespace TiendaVirtualNarvaez.Controllers
{
    public class ProductoController : Controller
    {
        private readonly TiendaContext _context; //context: conexion de los datos

        public ProductoController(TiendaContext context) //Inyeccion de dependencia
        {
            _context = context; //Accede a la tabla Productos
        }

        //LISTAR PRODUCTOS
        public IActionResult Index()
        {
            var productos = _context.Productos
                .Include(p => p.Categoria)
                .ToList();

            return View(productos);
        }

        // 2. FORMULARIO CREAR
        public IActionResult Create()
        {
            // SelectList (Lista, ValorId, TextoAMostrar)
            ViewBag.Categorias = ObtenerListaConIds(); 
            return View();
        }

        //GUARDAR PRODUCTO
        [HttpPost]
        public IActionResult Create(Producto producto)
        {
            // Validar que la categoría exista
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

            _context.Productos.Add(producto);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        //FORMULARIO EDITAR
        public IActionResult Edit(int id)
        {
            var producto = _context.Productos.Find(id);
            ViewBag.Categorias = ObtenerListaConIds();

            return View(producto);
        }

        //ACTUALIZAR PRODUCTO
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

            return RedirectToAction("Index");
        }

        //ELIMINAR PRODUCTO
        public IActionResult Delete(int id)
        {
            var producto = _context.Productos.Find(id);

            if (producto != null)
            {
                _context.Productos.Remove(producto);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // EVITAR ERRORES DE CONVERSION
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