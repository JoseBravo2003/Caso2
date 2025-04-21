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
        public IActionResult Inscribirse(int eventoId, int usuarioId)
        {
            var evento = _context.Eventos.Include(e => e.Asistentes).FirstOrDefault(e => e.Id == eventoId);
            if (evento == null) return NotFound();

            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == usuarioId);
            if (usuario == null) return NotFound();

            var horaInicio = evento.Fecha.Add(evento.Hora);
            var horaFin = horaInicio.AddMinutes(evento.Duracion);

            var conflictos = _context.EventoUsuarios
    .Include(eu => eu.Evento)
    .Where(eu => eu.UsuarioId == usuarioId)
    .ToList()
    .Any(eu =>
    {
        var inicio = eu.Evento.Fecha.Add(eu.Evento.Hora);
        var fin = inicio.AddMinutes(eu.Evento.Duracion);
        return horaInicio < fin && horaFin > inicio;
    });


            if (conflictos)
            {
                TempData["Error"] = "Conflicto con otro evento inscrito.";
                return RedirectToAction("ListaEventos");
            }

            if (evento.Asistentes.Count >= evento.CupoMaximo)
            {
                TempData["Error"] = "Este evento ya alcanzó el cupo máximo.";
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

