using Caso2.Models;

namespace Caso2.Models
{
    public class Categoria
    {

        public int Id { get; set; }


        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public bool? Estado { get; set; }

        public DateTime? FechaRegistro { get; set; } = DateTime.Now;

        public int UsuarioRegistro { get; set; }

        public Usuario? UsuarioAdmin { get; set; }
        public ICollection<Evento>? Eventos { get; set; }

    }
}
