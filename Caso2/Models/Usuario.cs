using Caso2.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Caso2.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Este campo es requerido.")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "Este campo es requerido.")]
        public string NombreCompleto { get; set; }
        [Required(ErrorMessage = "Este campo es requerido.")]
        public string Correo { get; set; }
        [Required(ErrorMessage = "Este campo es requerido.")]
        public string Telefono { get; set; }


        [Required(ErrorMessage = "Este campo es requerido.")]
        [RegularExpression(@"^(?=\w*\d)(?=\w*[A-Z])(?=\w*[a-z])\S{8,16}$", ErrorMessage = "La contraseña debe tener entre 8 y 16 caracteres, al menos un dígito, al menos una minúscula y al menos una mayúscula")]
        public string Contraseña { get; set; }
        public string Rol { get; set; } = "Usuario";  // Administrador, Organizador, Usuario.

        public bool ACtivo { get; set; } = true;

        public ICollection<Categoria>? Categorias { get; set; }
        public ICollection<Evento>? Eventos { get; set; }

        public ICollection<EventoUsuario> Asistencias { get; set; }



    }

}
