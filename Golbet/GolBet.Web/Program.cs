using GolBet.Repositories.Data;
using Microsoft.EntityFrameworkCore;
using GolBet.Repositories.Implementations;
using GolBet.Repositories.Interfaces;
using GolBet.Services.Implementations;
using GolBet.Services.Interfaces;
using GolBet.Services.Mapping;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

// Registrar DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro genérico abierto: una línea, un repositorio para cada entidad
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// AutoMapper: escanea el ensamblado de MappingProfile buscando todos los perfiles
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Servicios de negocio
builder.Services.AddScoped<IMatchService, MatchService>();

// Repositorios específicos
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
var app = builder.Build();

// Sembrar la base de datos al arrancar
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
