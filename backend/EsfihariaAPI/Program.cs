using EsfihariaAPI.Context;
using EsfihariaAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// RENDER PORT

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";

builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// DATABASE

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

var serverVersion =
    ServerVersion.AutoDetect(connectionString);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        serverVersion
    )
);

// CONTROLLERS

builder.Services.AddControllers();

// SWAGGER

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

// SERVICES

builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddScoped<
    IPasswordHashService,
    PasswordHashService>();

// JWT

builder.Services
.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true,

            ValidateAudience = true,

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,

            ValidIssuer =
                builder.Configuration["Jwt:Issuer"],

            ValidAudience =
                builder.Configuration["Jwt:Audience"],

            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        builder.Configuration["Jwt:SecretKey"]!
                    )
                ),

            ClockSkew = TimeSpan.Zero
        };
});

builder.Services.AddAuthorization();

// CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins(
                "https://SEU_FRONT.vercel.app",
                "https://SEU_FRONT.azurestaticapps.net"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// SWAGGER

app.UseSwagger();

app.UseSwaggerUI();

// HTTPS

app.UseHttpsRedirection();

// CORS

app.UseCors("Frontend");

// AUTH

app.UseAuthentication();

app.UseAuthorization();

// ROUTES

app.MapControllers();

app.Run();