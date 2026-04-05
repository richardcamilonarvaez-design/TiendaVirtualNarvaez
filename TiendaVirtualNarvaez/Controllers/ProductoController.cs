using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaVirtualNarvaez.Data;
using TiendaVirtualNarvaez.Models;
using TiendaVirtualNarvaez.Data;
using TiendaVirtualNarvaez.Models;

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

<<<<<<< HEAD
        // 2. FORMULARIO CREAR
        public IActionResult Create()
        {
            // SelectList (Lista, ValorId, TextoAMostrar)
            ViewBag.Categorias = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Categorias, "Id", "Nombre");

=======
        //FORMULARIO CREAR
        public IActionResult Create()
        {
            ViewBag.Categorias = _context.Categorias.ToList(); //Select
>>>>>>> eb6aa5aef1d4c1a475c61204ce7e404f9c460ba8
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

            // Validar modelo (precio, stock, etc.)
            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = _context.Categorias.ToList();
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
            ViewBag.Categorias = _context.Categorias.ToList();

            return View(producto);
        }

        //ACTUALIZAR PRODUCTO
        [HttpPost]
        public IActionResult Edit(Producto producto)
        {
            _context.Productos.Update(producto);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        //ELIMINAR PRODUCTO
        public IActionResult Delete(int id)
        {
            var producto = _context.Productos.Find(id);

            _context.Productos.Remove(producto);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
