using Microsoft.AspNetCore.Mvc;
using TiendaVirtualNarvaez.Models;
using TiendaVirtualNarvaez.Data;
using Microsoft.EntityFrameworkCore;

namespace TiendaVirtualNarvaez.Controllers
{
    public class ProductoController : Controller
    {
        private readonly TiendaContext _context;
        public ProductoController(TiendaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var productos = _context.Productos
            .Include(p => p.Categoria) 
            .ToList();
            return View(productos); 
        }
    }
}
