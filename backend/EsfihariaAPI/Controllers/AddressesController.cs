using EsfihariaAPI.Context;
using EsfihariaAPI.DTOs;
using EsfihariaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EsfihariaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly AppDbContext _db;

        public AddressesController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/Addresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddressListDTO>>> GetAll()
        {
            var addresses = await _db.Addresses
                .AsNoTracking()
                .Select(a => new AddressListDTO
                {
                    Id = a.Id,
                    IdUser = a.IdUser,
                    Address = a.Address,
                    Number = a.Number,
                    Neighborhood = a.Neighborhood,
                    Cep = a.Cep,
                    Complement = a.Complement,
                    Landmark = a.Landmark
                })
                .ToListAsync();

            return Ok(addresses);
        }

        // GET: api/Addresses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AddressListDTO>> GetById(int id)
        {
            var address = await _db.Addresses
                .AsNoTracking()
                .Where(a => a.Id == id)
                .Select(a => new AddressListDTO
                {
                    Id = a.Id,
                    IdUser = a.IdUser,
                    Address = a.Address,
                    Number = a.Number,
                    Neighborhood = a.Neighborhood,
                    Cep = a.Cep,
                    Complement = a.Complement,
                    Landmark = a.Landmark
                })
                .FirstOrDefaultAsync();

            if (address == null)
                return NotFound("Endereço não encontrado.");

            return Ok(address);
        }

        // GET: api/Addresses/5
        [HttpGet("user/{idUser}")]
        public async Task<ActionResult<AddressListDTO>> GetByIdUser(int idUser)
        {
            var address = await _db.Addresses
                .AsNoTracking()
                .Where(a => a.IdUser == idUser)
                .Select(a => new AddressListDTO
                {
                    Id = a.Id,
                    IdUser = a.IdUser,
                    Address = a.Address,
                    Number = a.Number,
                    Neighborhood = a.Neighborhood,
                    Cep = a.Cep,
                    Complement = a.Complement,
                    Landmark = a.Landmark
                })
                .ToListAsync();

            if (address == null)
                return NotFound("Usuário não encontrado.");

            return Ok(address);
        }

        // POST: api/Addresses
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateAddressDTO dto)
        {
            var userExists = await _db.Users.AnyAsync(u => u.Id == dto.IdUser);

            if (!userExists)
                return BadRequest("Usuário informado não existe.");

            var address = new Addresses
            {
                IdUser = dto.IdUser,
                Address = dto.Address,
                Number = dto.Number,
                Neighborhood = dto.Neighborhood,
                Cep = dto.Cep,
                Complement = dto.Complement,
                Landmark = dto.Landmark
            };

            _db.Addresses.Add(address);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = address.Id }, dto);
        }

        // PUT: api/Addresses/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateAddressDTO dto)
        {
            var address = await _db.Addresses.FindAsync(id);

            if (address == null)
                return NotFound("Endereço não encontrado.");

            address.Address = dto.Address;
            address.Number = dto.Number;
            address.Neighborhood = dto.Neighborhood;
            address.Cep = dto.Cep;
            address.Complement = dto.Complement;
            address.Landmark = dto.Landmark;

            await _db.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Addresses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var address = await _db.Addresses.FindAsync(id);

            if (address == null)
                return NotFound("Endereço não encontrado.");

            _db.Addresses.Remove(address);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}