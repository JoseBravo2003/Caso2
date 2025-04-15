
using Caso2.Models;
using Microsoft.EntityFrameworkCore;

namespace Caso2.Models // <-- cambia por el namespace real de tu proyecto
{
    public class EventCorpDbContext : DbContext
    {
        public EventCorpDbContext(DbContextOptions<EventCorpDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<EventoUsuario> EventoUsuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔁 Relación muchos a muchos entre Usuario y Evento (asistencia)
            modelBuilder.Entity<EventoUsuario>()
                .HasKey(eu => new { eu.UsuarioId, eu.EventoId });

            modelBuilder.Entity<EventoUsuario>()
                .HasOne(eu => eu.Usuario)
                .WithMany(u => u.Asistencias)
                .HasForeignKey(eu => eu.UsuarioId);

            modelBuilder.Entity<EventoUsuario>()
                .HasOne(eu => eu.Evento)
                .WithMany(e => e.Asistentes)
                .HasForeignKey(eu => eu.EventoId);

            // 📎 Relación Evento -> Categoria
            modelBuilder.Entity<Evento>()
                .HasOne(e => e.Categoria)
                .WithMany(c => c.Eventos)
                .HasForeignKey(e => e.IdCategoria)
                .OnDelete(DeleteBehavior.Restrict); // Opcional, para evitar eliminaciones en cascada

            // 📎 Relación Evento -> Usuario (creador del evento)
            modelBuilder.Entity<Evento>()
                .HasOne(e => e.Usuario)
                .WithMany(u => u.Eventos)
                .HasForeignKey(e => e.UsuarioRegistro)
                .OnDelete(DeleteBehavior.Restrict);

            // 📎 Relación Categoria -> Usuario (creador de la categoría)
            modelBuilder.Entity<Categoria>()
                .HasOne(c => c.UsuarioAdmin)
                .WithMany(u => u.Categorias)
                .HasForeignKey(c => c.UsuarioRegistro)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
