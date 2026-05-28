using EsfihariaAPI.Context;
using EsfihariaAPI.DTOs;
using EsfihariaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EsfihariaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserController(AppDbContext db)
        {
            _db = db;
            _passwordHasher = new PasswordHasher<User>();
        }

        // GET: api/User
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserListDTO>>> GetAll()
        {
            var users = await _db.Users
                .Select(u => new UserListDTO
                {
                    Id = u.Id,
                    IdRole = u.IdRole,
                    Name = u.Name,
                    Email = u.Email,
                    Phone = u.Phone
                })
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/User/5
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserListDTO>> GetById(int id)
        {
            var user = await _db.Users
                .Where(u => u.Id == id)
                .Select(u => new UserListDTO
                {
                    Id = u.Id,
                    IdRole = u.IdRole,
                    Name = u.Name,
                    Email = u.Email,
                    Phone = u.Phone
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound("Usuário não encontrado.");

            return Ok(user);
        }

        // POST: api/User/Login
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == loginDTO.Email);

            if (user == null)
                return Unauthorized("Email ou senha inválidos.");

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, loginDTO.Password);

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Email ou senha inválidos.");

            return Ok(new
            {
                message = "Login realizado com sucesso.",
                user = new
                {
                    user.Id,
                    user.Name,
                    user.Email,
                    user.Phone,
                    user.IdRole
                }
            });
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] SignUpDTO cadastroDTO)
        {
            var emailExists = await _db.Users.AnyAsync(u => u.Email == cadastroDTO.Email);

            if (emailExists)
                return Conflict("Já existe um usuário com esse email.");

            // Validação extra para motoristas
            if (cadastroDTO.IdRole == 3)
            {
                if (string.IsNullOrWhiteSpace(cadastroDTO.Cnh) ||
                    string.IsNullOrWhiteSpace(cadastroDTO.LicensePlate))
                {
                    return BadRequest("Motoristas precisam informar CNH e Placa do veículo.");
                }
            }

            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                var user = new User
                {
                    IdRole = cadastroDTO.IdRole,
                    Name = cadastroDTO.Name,
                    Email = cadastroDTO.Email,
                    Phone = cadastroDTO.Phone
                };

                user.Password = _passwordHasher.HashPassword(user, cadastroDTO.Password);

                _db.Users.Add(user);
                await _db.SaveChangesAsync(); // Aqui o Id do usuário é gerado

                // SE FOR MOTORISTA, CRIA O DRIVER
                if (cadastroDTO.IdRole == 3)
                {
                    var driver = new Driver
                    {
                        IdUser = user.Id,
                        IdStatus = 1, // Ativo por padrão
                        Cnh = cadastroDTO.Cnh!,
                        LicensePlate = cadastroDTO.LicensePlate!
                    };

                    _db.Drivers.Add(driver);
                    await _db.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = user.Id },
                    new
                    {
                        user.Id,
                        user.Name,
                        user.Email,
                        user.Phone,
                        user.IdRole
                    }
                );
            }
            catch
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Erro ao criar usuário.");
            }
        }

        // PUT: api/User/5
        [Authorize(Roles = "Admin,Customer")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateUserDTO updateDTO)
        {
            var user = await _db.Users.FindAsync(id);

            if (user == null)
                return NotFound("Usuário não encontrado.");

            var emailTaken = await _db.Users.AnyAsync(u => u.Email == updateDTO.Email && u.Id != id);

            if (emailTaken)
                return Conflict("Já existe outro usuário com esse email.");

            user.Name = updateDTO.Name;
            user.Email = updateDTO.Email;
            user.Phone = updateDTO.Phone;
            user.IdRole = updateDTO.IdRole;

            await _db.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/User/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var user = await _db.Users.FindAsync(id);

            if (user == null)
                return NotFound("Usuário não encontrado.");

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}