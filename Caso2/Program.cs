using Microsoft.EntityFrameworkCore; // Importar Entity Framework Core
using Caso2.Models;
using Microsoft.EntityFrameworkCore.Migrations;

var builder = WebApplication.CreateBuilder(args);

// Configurar el DbContext con la cadena de conexi�n desde appsettings.json
builder.Services.AddDbContext<EventCorpDbContext>(op =>
{
    op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// Habilitar el uso de sesiones
builder.Services.AddDistributedMemoryCache(); // Necesario para almacenar sesiones en memoria
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1); // Tiempo de expiraci�n de la sesi�n
    options.Cookie.HttpOnly = true; // Asegura que la cookie de sesi�n no sea accesible por scripts
    options.Cookie.IsEssential = true; // Necesario para que funcione en GDPR-compliance
});

// Agregar servicios para controladores y vistas
builder.Services.AddControllersWithViews();

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

// Configuraci�n del pipeline de solicitudes HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Habilitar HSTS (HTTP Strict Transport Security)
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Middleware para usar sesiones

app.UseAuthorization();

// Configurar la ruta predeterminada
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Acceso}/{action=InicioSesion}/{id?}");

app.Run();


