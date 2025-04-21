using Microsoft.AspNetCore.Mvc;
using Caso2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;

namespace Caso2.Controllers
{
    public class AccesoController : Controller
    {
        private readonly EventCorpDbContext _context;
        public AccesoController(EventCorpDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult InicioSesion()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult InicioSesion(LoginModeloUsuario modelo)
        {
            if (ModelState.IsValid)
            {
                var usuario = _context.Usuarios
                    .FirstOrDefault(u => u.Correo == modelo.Correo && u.Contraseña == modelo.Contraseña);

                if (usuario != null)
                {
                    HttpContext.Session.SetString("UsuarioId", usuario.Id.ToString());
                    HttpContext.Session.SetString("UsuarioNombre", usuario.NombreCompleto);
                    HttpContext.Session.SetString("UsuarioRol", usuario.Rol.ToString());

                    TempData["Mensaje"] = $"Bienvenido {usuario.NombreUsuario}";
                    return RedirectToAction("Index", "Home");
                }

                TempData["MensajeInicioFallido"] = "Correo o contraseña incorrectos.";
                return RedirectToAction("InicioSesion");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult RegistroUsuario()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult RegistroUsuario(Usuario modelo)
        {
            if (ModelState.IsValid)
            {
                // Validar que el usuario no exista previamente
                if (_context.Usuarios.Any(u => u.NombreUsuario == modelo.NombreUsuario || u.Correo == modelo.Correo))
                {
                    TempData["MensajeRegistroIncorrecto"] = "El nombre de usuario o correo ya están registrados.";
                    return View(modelo);
                }

                // Crear un nuevo usuario con la información proporcionada
                var nuevoUsuario = new Usuario
                {
                    NombreUsuario = modelo.NombreUsuario,
                    NombreCompleto = modelo.NombreCompleto,
                    Correo = modelo.Correo,
                    Telefono = modelo.Telefono,
                    Contraseña = modelo.Contraseña,
                    Rol = modelo.Rol,
                    ACtivo = modelo.ACtivo
                };

                // Guardar en la base de datos
                _context.Usuarios.Add(nuevoUsuario);
                _context.SaveChanges();

                TempData["MensajeRegistroCorrecto"] = "Usuario registrado correctamente, ahora puedes iniciar sesión.";
                return RedirectToAction("RegistroUsuario");
            }

            return View(modelo);
        }

        [HttpGet]
        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("InicioSesion", "Acceso");
        }

    }
}