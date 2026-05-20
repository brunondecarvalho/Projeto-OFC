using EsfihariaAPI.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var serverVersion = ServerVersion.AutoDetect(connectionString);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, serverVersion));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// --- 1. REGISTRAR OS SERVIÇOS DO SWAGGER ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Essencial para mapear as rotas
builder.Services.AddSwaggerGen();           // Gera a documentação OpenAPI

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();   // Gera o arquivo json com as especificações da API
    app.UseSwaggerUI(); // Ativa a interface visual no navegador
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
