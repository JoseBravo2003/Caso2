using Caso2.Models;
using System.ComponentModel.DataAnnotations;

namespace Caso2.Models
{
    public class Evento
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Este campo es requerido.")]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "Este campo es requerido.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Este campo es requerido.")]
        public string Ubicacion { get; set; }


        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.Time)]
        public TimeSpan Hora { get; set; }

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Range(1, 1000, ErrorMessage = "Duración debe ser entre 1 y 1000 minutos.")]
        public int Duracion { get; set; } // En minutos

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Range(1, 1000, ErrorMessage = "Cupo debe ser entre 1 y 1000.")]
        public int CupoMaximo { get; set; }



        public ICollection<EventoUsuario> Asistentes { get; set; } = new List<EventoUsuario>();




        public DateTime? FechaRegistro { get; set; } = DateTime.Now;

        public Categoria? Categoria { get; set; }
        public int IdCategoria { get; set; }

        public Usuario? Usuario { get; set; }
        public int UsuarioRegistro { get; set; }

    }
}
