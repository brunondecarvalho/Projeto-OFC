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
    private readonly IJwtService _jwt;
    private readonly IPasswordHashService _hash;

    public AuthController(
        AppDbContext db,
        IJwtService jwt,
        IPasswordHashService hash)
    {
        _db = db;
        _jwt = jwt;
        _hash = hash;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginDTO dto)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(
                x => x.Email == dto.Email
            );

        if (user == null)
            return Unauthorized();

        var valid = _hash.Verify(
            user,
            user.Password,
            dto.Password
        );

        if (!valid)
            return Unauthorized();

        var token = _jwt.GenerateToken(user);

        return Ok(new { token });
    }
}