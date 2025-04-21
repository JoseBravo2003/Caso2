using Caso2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Caso2.Controllers
{
    public class InscripcionController : Controller
    {
        private readonly EventCorpDbContext _context;

        public InscripcionController(EventCorpDbContext context)
        {
            _context = context;
        }

        public IActionResult ListaEventos()
        {
            var eventos = _context.Eventos
                .Include(e => e.Categoria)
                .Include(e => e.Asistentes)
                .ToList();

            return View(eventos);
        }
        [HttpPost]
        public IActionResult Inscribirse(int eventoId)
        {
            var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioIdStr))
            {
                TempData["Error"] = "Debe iniciar sesión para inscribirse.";
                return RedirectToAction("InicioSesion", "Acceso");
            }

            int usuarioId = int.Parse(usuarioIdStr);

            // Verificar que no esté ya inscrito
            bool yaInscrito = _context.EventoUsuarios.Any(eu => eu.EventoId == eventoId && eu.UsuarioId == usuarioId);
            if (yaInscrito)
            {
                TempData["Error"] = "Ya estás inscrito en este evento.";
                return RedirectToAction("ListaEventos");
            }

            var inscripcion = new EventoUsuario
            {
                UsuarioId = usuarioId,
                EventoId = eventoId,
                FechaAsistencia = DateTime.Now
            };

            _context.EventoUsuarios.Add(inscripcion);
            _context.SaveChanges();

            TempData["Exito"] = "Inscripción exitosa.";
            return RedirectToAction("ListaEventos");
        }


        public IActionResult VerInscritos(int eventoId)
        {
            var inscritos = _context.EventoUsuarios
                .Include(eu => eu.Usuario)
                .Where(eu => eu.EventoId == eventoId)
                .ToList();

            return View(inscritos);
        }

        public IActionResult EventosPorOrganizador(int usuarioId)
        {
            var eventos = _context.Eventos
                .Where(e => e.UsuarioRegistro == usuarioId)
                .Include(e => e.Asistentes)
                .ThenInclude(a => a.Usuario)
                .ToList();

            return View(eventos);
        }
    }
}
