using Caso2.Models;
using Microsoft.AspNetCore.Mvc;

namespace Caso2.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly EventCorpDbContext _context;

        public CategoriasController(EventCorpDbContext context)
        {
            _context = context;
        }

        public IActionResult Crear()
        {
            ViewBag.Usuarios = _context.Usuarios.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Usuarios = _context.Usuarios.ToList();
                return View(categoria);
            }

            categoria.FechaRegistro = DateTime.Now;
            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            TempData["Exito"] = "Categoría creada exitosamente.";
            return RedirectToAction("Crear");
        }
    }
}
