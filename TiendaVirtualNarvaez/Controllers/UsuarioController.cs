using Microsoft.AspNetCore.Mvc;
using TiendaVirtualNarvaez.Data;
using TiendaVirtualNarvaez.Models;

namespace TiendaVirtualNarvaez.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly TiendaContext _context;

        public UsuarioController(TiendaContext context)
        {
            _context = context;
        }

        //LISTAR USUARIOS
        public IActionResult Index()
        {
            var usuarios = _context.Usuarios.ToList();
            return View(usuarios);
        }

        //FORMULARIO CREAR
        public IActionResult Create()
        {
            return View();
        }

        //GUARDAR USUARIO
        [HttpPost]
        public IActionResult Create(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return View(usuario);
            }

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        //FORMULARIO EDITAR
        public IActionResult Edit(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            return View(usuario);
        }

        //ACTUALIZAR USUARIO
        [HttpPost]
        public IActionResult Edit(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return View(usuario);
            }

            _context.Usuarios.Update(usuario);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        //ELIMINAR USUARIO
        public IActionResult Delete(int id)
        {
            var usuario = _context.Usuarios.Find(id);

            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
