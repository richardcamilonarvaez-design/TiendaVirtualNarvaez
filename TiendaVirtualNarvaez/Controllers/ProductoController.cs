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
                .Include(p => p.Categoria) //Carga la relacion con la tabla categoria, no va traer los datos de la tabla categorias
                .ToList(); //Ejecuta la consulta

            return View(productos);
        }

        //FORMULARIO CREAR

        public IActionResult Create()
        {
            return View();
        }

        //GUARDAR PRODUCTO   --Metodo Post 
        [HttpPost]

        public IActionResult Create(Producto producto)
        {
            _context.Productos.Add(producto); //Conexion con la base de datos
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        //FORMULARIO EDITAR

        public IActionResult Edit(int id) //Metodo get por defecto
        {
            var producto = _context.Productos.Find(id);
            ViewBag.Categorias = _context.Categorias.ToList(); //Traer las tablas de categorias

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
