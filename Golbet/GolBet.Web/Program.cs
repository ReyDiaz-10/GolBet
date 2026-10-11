// GolBet.Web/Program.cs

using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GolBet.Entities;
using GolBet.Repositories.Data;
using GolBet.Repositories.Implementations;
using GolBet.Repositories.Interfaces;
using GolBet.Services.Implementations;
using GolBet.Services.Interfaces;
using GolBet.Services.Mapping;

// 1. Configuración de Cultura (Módulo 6: Todo en español Colombia)
var culture = new CultureInfo("es-CO");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

var builder = WebApplication.CreateBuilder(args);

// 2. Agregar controladores y vistas
builder.Services.AddControllersWithViews();

// 3. Registrar DbContext (Módulo 2)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 4. Registrar Identity y configurar Cookies (Módulo 7)
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    // Política de contraseñas amigable para ámbito académico
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = true;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";                // Si no hay sesión
    options.AccessDeniedPath = "/Account/AccessDenied";  // Si no tiene el rol
});

// 5. Registrar Repositorios (Módulo 3)
// Registro genérico abierto
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
// Repositorios específicos
builder.Services.AddScoped<IMatchRepository, MatchRepository>();

// 6. Registrar AutoMapper (Módulo 4)
builder.Services.AddAutoMapper(typeof(MappingProfile));

// 7. Registrar Servicios de Negocio (Módulos 4 y 6)
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<ITeamService, TeamService>();

// ==========================================================
// CONSTRUCCIÓN DE LA APLICACIÓN
// ==========================================================
var app = builder.Build();

// 8. Sembrar la base de datos al arrancar (Seeder Módulo 7)
// Esto debe ir después de builder.Build() y antes del pipeline HTTP.
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    await DbSeeder.SeedAsync(context, roleManager, userManager);
}

// 9. Configuración del Pipeline HTTP (Middlewares)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseRouting();

// ORDEN CRÍTICO DE SEGURIDAD (Módulo 7)
app.UseAuthentication();   // Primero: ¿Quién eres? (Lee la cookie de Identity)
app.UseAuthorization();    // Segundo: ¿Qué puedes hacer? (Evalúa [Authorize] y Roles)

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// 10. Arrancar el servidor web (Debe ser SIEMPRE la última línea)
app.Run();