using Caso2.Models;
using Microsoft.EntityFrameworkCore;

namespace Caso2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<EventCorpDbContext>(op =>
            {
                op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.MapGet("/api/events", async (EventCorpDbContext db) =>
            {
                return await db.Eventos
                    .Select(e => new {
                        e.Id,
                        e.Titulo,
                        e.Descripcion,
                        e.Fecha,
                        e.Hora,
                        e.Ubicacion,
                        e.CupoMaximo
                    }).ToListAsync();
            });

            app.MapGet("/api/events/{id}", async (int id, EventCorpDbContext db) =>
            {
                var evento = await db.Eventos
                    .Where(e => e.Id == id)
                    .Select(e => new {
                        e.Id,
                        e.Titulo,
                        e.Descripcion,
                        e.Fecha,
                        e.Hora,
                        e.Ubicacion,
                        e.CupoMaximo
                    }).FirstOrDefaultAsync();

                return evento is null ? Results.NotFound() : Results.Ok(evento);
            });


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
