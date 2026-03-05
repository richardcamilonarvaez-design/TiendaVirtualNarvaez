using Microsoft.AspNetCore.Mvc;
using TiendaVirtualNarvaez.Models;

namespace TiendaVirtualNarvaez.Controllers
{
    public class CategoriaController : Controller
    {
        public IActionResult Index()
        {
            var categoria = new List<Categoria>
            {
                new Categoria { Nombre= "Tegnologia", Descripcion="Elementos Tecnologicos", Estado="Activo" },
                new Categoria { Nombre= "Ropa", Descripcion="Prendas de Ropa", Estado="Activo"},
                new Categoria { Nombre= "", Descripcion="", Estado="Inactivo"}
            };
            return View(categoria);
        }
    }
}
