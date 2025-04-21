using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Caso2.Models;

namespace Caso2.Controllers
{
    public class EventoController : Controller
    {
        private readonly EventCorpDbContext _context;

        public EventoController(EventCorpDbContext context)
        {
            _context = context;
        }

        // GET: Evento
        public async Task<IActionResult> Index()
        {
            var eventCorpDbContext = _context.Eventos.Include(e => e.Categoria).Include(e => e.Usuario);
            return View(await eventCorpDbContext.ToListAsync());
        }
        
        public async Task<IActionResult> IndexCliente()
        {
            var eventCorpDbContext = _context.Eventos.Include(e => e.Categoria).Include(e => e.Usuario);
            return View(await eventCorpDbContext.ToListAsync());
        }

        // GET: Evento/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos
                .Include(e => e.Categoria)
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        // GET: Evento/Create
        public IActionResult Create()
        {
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "Id", "Id");
            ViewData["UsuarioRegistro"] = new SelectList(_context.Usuarios, "Id", "Correo");
            return View();
        }

        // POST: Evento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Descripcion,Ubicacion,Fecha,Hora,Duracion,CupoMaximo,FechaRegistro,IdCategoria,UsuarioRegistro")] Evento evento)
        {
            if (ModelState.IsValid)
            {
                evento.FechaRegistro = DateTime.Now;
                _context.Add(evento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "Id", "Id", evento.IdCategoria);
            ViewData["UsuarioRegistro"] = new SelectList(_context.Usuarios, "Id", "Correo", evento.UsuarioRegistro);
            return View(evento);
        }

        // GET: Evento/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "Id", "Id", evento.IdCategoria);
            ViewData["UsuarioRegistro"] = new SelectList(_context.Usuarios, "Id", "Correo", evento.UsuarioRegistro);
            return View(evento);
        }

        // POST: Evento/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descripcion,Ubicacion,Fecha,Hora,Duracion,CupoMaximo,FechaRegistro,IdCategoria,UsuarioRegistro")] Evento evento)
        {
            if (id != evento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(evento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventoExists(evento.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "Id", "Id", evento.IdCategoria);
            ViewData["UsuarioRegistro"] = new SelectList(_context.Usuarios, "Id", "Correo", evento.UsuarioRegistro);
            return View(evento);
        }

        // GET: Evento/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos
                .Include(e => e.Categoria)
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        // POST: Evento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento != null)
            {
                _context.Eventos.Remove(evento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventoExists(int id)
        {
            return _context.Eventos.Any(e => e.Id == id);
        }
    }
}
