using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using PCBuilder.Infrastructure.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// ... código anterior ...

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});
builder.Services.AddSwaggerGen();

// Esto ya lo pusimos, pero verificalo
builder.Services.AddValidatorsFromAssemblyContaining<PCBuilder.API.Validators.ProcessorValidator>();

// Esto hace que la API devuelva automáticamente un 400 con los errores del Validador
builder.Services.AddFluentValidationAutoValidation();
// ¡NUEVA CONFIGURACIÓN DE BASE DE DATOS!
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(PCBuilder.Domain.Interfaces.IRepository<>), typeof(PCBuilder.Infrastructure.Repositories.Repository<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();   // <-- NUEVA LÍNEA
    app.UseSwaggerUI(); // <-- NUEVA LÍNEA
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
