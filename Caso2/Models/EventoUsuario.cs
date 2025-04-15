using Caso2.Models;

namespace Caso2.Models
{
    public class EventoUsuario
    {
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public int EventoId { get; set; }
        public Evento Evento { get; set; }

        public DateTime FechaAsistencia { get; set; } = DateTime.Now;
    }
}
