using Caso2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Caso2.Controllers
{
    public class EventosController : Controller
    {
        private readonly EventCorpDbContext _context;

        public EventosController(EventCorpDbContext context)
        {
            _context = context;
        }

        public IActionResult Crear()
        {
            ViewBag.Categorias = _context.Categorias.ToList();
            ViewBag.Usuarios = _context.Usuarios.Where(u => u.Rol == "Organizador").ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Evento evento)
        {
            ViewBag.Categorias = _context.Categorias.ToList();
            ViewBag.Usuarios = _context.Usuarios.Where(u => u.Rol == "Organizador").ToList();

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "El formulario contiene errores.";
                return View(evento);
            }

            // Validaciones adicionales
            if (evento.IdCategoria == 0 || evento.UsuarioRegistro == 0)
            {
                ModelState.AddModelError("", "Debe seleccionar una categoría y un organizador.");
                return View(evento);
            }

            evento.FechaRegistro = DateTime.Now;

            try
            {
                _context.Eventos.Add(evento);
                _context.SaveChanges();
                TempData["Exito"] = "Evento creado correctamente.";
                return RedirectToAction("ListaEventos", "Inscripcion");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al guardar en la base de datos: " + ex.Message);
                return View(evento);
            }
        }

    }
}
