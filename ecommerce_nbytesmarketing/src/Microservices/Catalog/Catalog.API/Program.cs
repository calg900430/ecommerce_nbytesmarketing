using Catalog.API.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Accedemos a las diferentes cadenas de conexion,
//para la base de datos local, la de docker y la de azure.
// Detecta el entorno y carga las configuraciones correspondientes
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Archivo base
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true) // Configuración específica
    .AddEnvironmentVariables();

// Obtiene la cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


//Inyectamos el DataContext
builder.Services.AddDbContext<DataContext>(
    x => x.UseSqlServer(connectionString)
    );



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
