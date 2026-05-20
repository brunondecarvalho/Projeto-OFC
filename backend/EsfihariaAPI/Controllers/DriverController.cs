using EsfihariaAPI.Context;
using EsfihariaAPI.DTOs;
using EsfihariaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EsfihariaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly AppDbContext _db;

        public DriverController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/Driver
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriverlistDTO>>> GetAll()
        {
            var drivers = await _db.Drivers
                .Select(d => new DriverlistDTO
                {
                    Id = d.Id,
                    IdUser = d.IdUser,
                    CNH = d.Cnh,
                    LicensePlate = d.LicensePlate,
                    IdStatus = d.IdStatus
                })
                .ToListAsync();

            return Ok(drivers);
        }

        // GET: api/Driver/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserListDTO>> GetById(int id)
        {
            var driver = await _db.Drivers
                .Where(d => d.Id == id)
                .Select(d => new DriverlistDTO
                {
                    Id = d.Id,
                    IdUser = d.IdUser,
                    CNH = d.Cnh,
                    LicensePlate = d.LicensePlate,
                    IdStatus = d.IdStatus
                })
                .FirstOrDefaultAsync();

            if (driver == null)
                return NotFound("Motorista não encontrado.");

            return Ok(driver);
        }

        // PUT: api/Driver/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateDriverDTO updateDTO)
        {
            var driver = await _db.Drivers.FindAsync(id);

            if (driver == null)
                return NotFound("Motorista não encontrado.");

            driver.Cnh = updateDTO.CNH;
            driver.LicensePlate = updateDTO.LicensePlate;

            await _db.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Driver/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var driver = await _db.Drivers.FindAsync(id);

            if (driver == null)
                return NotFound("Motorista não encontrado.");

            _db.Drivers.Remove(driver);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
