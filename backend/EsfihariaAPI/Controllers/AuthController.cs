using EsfihariaAPI.Context;
using EsfihariaAPI.DTOs;
using EsfihariaAPI.Models;
using EsfihariaAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EsfihariaAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;

    private readonly IJwtService _jwtService;

    private readonly IPasswordHashService _passwordHashService;

    public AuthController(
        AppDbContext db,
        IJwtService jwtService,
        IPasswordHashService passwordHashService)
    {
        _db = db;
        _jwtService = jwtService;
        _passwordHashService = passwordHashService;
    }

    // LOGIN

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginDTO dto)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(
                x => x.Email == dto.Email
            );

        if (user == null)
            return Unauthorized(
                "Email ou senha inválidos."
            );

        var validPassword =
            _passwordHashService.Verify(
                user,
                user.Password,
                dto.Password
            );

        if (!validPassword)
            return Unauthorized(
                "Email ou senha inválidos."
            );

        var token =
            _jwtService.GenerateToken(user);

        return Ok(new
        {
            Token = token,
            User = new
            {
                user.Id,
                user.Name,
                user.Email,
                user.IdRole
            }
        });
    }

    // REGISTER

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] SignUpDTO dto)
    {
        var emailExists = await _db.Users
            .AnyAsync(x => x.Email == dto.Email);

        if (emailExists)
            return Conflict(
                "Email já cadastrado."
            );

        var user = new User
        {
            IdRole = 1,
            Name = dto.Name.Trim(),
            Email = dto.Email.Trim().ToLower(),
            Phone = dto.Phone.Trim()
        };

        user.Password =
            _passwordHashService.Hash(
                user,
                dto.Password
            );

        _db.Users.Add(user);

        await _db.SaveChangesAsync();

        return Created(string.Empty, new
        {
            user.Id,
            user.Name,
            user.Email
        });
    }

    // ME

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            Id = User.Claims.FirstOrDefault(
                x => x.Type.Contains("nameidentifier")
            )?.Value,

            Name = User.Identity?.Name,

            Email = User.Claims.FirstOrDefault(
                x => x.Type.Contains("emailaddress")
            )?.Value,

            Role = User.Claims.FirstOrDefault(
                x => x.Type.Contains("role")
            )?.Value
        });
    }
}