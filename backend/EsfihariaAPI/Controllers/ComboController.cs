using EsfihariaAPI.Context;
using EsfihariaAPI.DTOs;
using EsfihariaAPI.DTOs.EsfihariaAPI.DTOs;
using EsfihariaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EsfihariaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComboController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ComboController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/Combo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComboListDTO>>> GetAll()
        {
            var combos = await _db.Combos
                .AsNoTracking()
                .Select(c => new ComboListDTO
                {
                    Id = c.Id,
                    IdStatus = c.IdStatus,
                    Name = c.Name,
                    Description = c.Description,
                    Price = c.Price,
                    ItemsCount = _db.ComboProduct.Count(cp => cp.IdCombo == c.Id)
                })
                .ToListAsync();

            return Ok(combos);
        }

        // GET: api/Combo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ComboDetailsDTO>> GetById(int id)
        {
            var combo = await _db.Combos
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new ComboDetailsDTO
                {
                    Id = c.Id,
                    IdStatus = c.IdStatus,
                    Name = c.Name,
                    Description = c.Description,
                    Price = c.Price
                })
                .FirstOrDefaultAsync();

            if (combo == null)
                return NotFound("Combo não encontrado.");

            var items = await (
                from cp in _db.ComboProduct.AsNoTracking()
                join p in _db.Products.AsNoTracking() on cp.IdProduct equals p.Id
                where cp.IdCombo == id
                select new ComboProductDTO
                {
                    IdProduct = p.Id,
                    ProductName = p.Name,
                    Quantity = cp.Quantity,
                    UnitPrice = p.Price,
                    TotalPrice = p.Price * cp.Quantity
                }
            ).ToListAsync();

            combo.Products = items;

            return Ok(combo);
        }

        // GET: api/Combo/search?name=burger
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ComboListDTO>>> Search([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Informe um termo para busca.");

            var combos = await _db.Combos
                .AsNoTracking()
                .Where(c => c.Name.Contains(name))
                .Select(c => new ComboListDTO
                {
                    Id = c.Id,
                    IdStatus = c.IdStatus,
                    Name = c.Name,
                    Description = c.Description,
                    Price = c.Price,
                    ItemsCount = _db.ComboProduct.Count(cp => cp.IdCombo == c.Id)
                })
                .ToListAsync();

            return Ok(combos);
        }

        // POST: api/Combo
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateComboDTO dto)
        {
            if (dto.IdStatus <= 0)
                return BadRequest("Informe um status válido.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("O nome do combo é obrigatório.");

            if (dto.Price < 0)
                return BadRequest("O preço não pode ser negativo.");

            if (dto.Products == null || dto.Products.Count == 0)
                return BadRequest("O combo precisa ter pelo menos um produto.");

            if (dto.Products.Any(p => p.Quantity <= 0))
                return BadRequest("A quantidade de cada produto deve ser maior que zero.");

            var normalizedItems = dto.Products
                .GroupBy(p => p.IdProduct)
                .Select(g => new ComboItemDTO
                {
                    IdProduct = g.Key,
                    Quantity = g.Sum(x => x.Quantity)
                })
                .ToList();

            var productIds = normalizedItems.Select(x => x.IdProduct).ToList();
            var existingProducts = await _db.Products
                .Where(p => productIds.Contains(p.Id))
                .Select(p => p.Id)
                .ToListAsync();

            if (existingProducts.Count != productIds.Count)
                return BadRequest("Um ou mais produtos informados não existem.");

            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                var combo = new Combo
                {
                    IdStatus = dto.IdStatus,
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price
                };

                _db.Combos.Add(combo);
                await _db.SaveChangesAsync();

                var comboProducts = normalizedItems.Select(item => new ComboProduct
                {
                    IdCombo = combo.Id,
                    IdProduct = item.IdProduct,
                    Quantity = item.Quantity
                }).ToList();

                _db.ComboProduct.AddRange(comboProducts);
                await _db.SaveChangesAsync();

                await transaction.CommitAsync();

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = combo.Id },
                    new
                    {
                        combo.Id,
                        combo.IdStatus,
                        combo.Name,
                        combo.Description,
                        combo.Price
                    }
                );
            }
            catch
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Erro ao criar o combo.");
            }
        }

        // PUT: api/Combo/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateComboDTO dto)
        {
            if (dto.IdStatus <= 0)
                return BadRequest("Informe um status válido.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("O nome do combo é obrigatório.");

            if (dto.Price < 0)
                return BadRequest("O preço não pode ser negativo.");

            if (dto.Products == null || dto.Products.Count == 0)
                return BadRequest("O combo precisa ter pelo menos um produto.");

            if (dto.Products.Any(p => p.Quantity <= 0))
                return BadRequest("A quantidade de cada produto deve ser maior que zero.");

            var combo = await _db.Combos.FindAsync(id);

            if (combo == null)
                return NotFound("Combo não encontrado.");

            var normalizedItems = dto.Products
                .GroupBy(p => p.IdProduct)
                .Select(g => new ComboItemDTO
                {
                    IdProduct = g.Key,
                    Quantity = g.Sum(x => x.Quantity)
                })
                .ToList();

            var productIds = normalizedItems.Select(x => x.IdProduct).ToList();
            var existingProducts = await _db.Products
                .Where(p => productIds.Contains(p.Id))
                .Select(p => p.Id)
                .ToListAsync();

            if (existingProducts.Count != productIds.Count)
                return BadRequest("Um ou mais produtos informados não existem.");

            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                combo.IdStatus = dto.IdStatus;
                combo.Name = dto.Name;
                combo.Description = dto.Description;
                combo.Price = dto.Price;

                await _db.SaveChangesAsync();

                var oldItems = await _db.ComboProduct
                    .Where(cp => cp.IdCombo == id)
                    .ToListAsync();

                _db.ComboProduct.RemoveRange(oldItems);
                await _db.SaveChangesAsync();

                var newItems = normalizedItems.Select(item => new ComboProduct
                {
                    IdCombo = combo.Id,
                    IdProduct = item.IdProduct,
                    Quantity = item.Quantity
                }).ToList();

                _db.ComboProduct.AddRange(newItems);
                await _db.SaveChangesAsync();

                await transaction.CommitAsync();

                return NoContent();
            }
            catch
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Erro ao atualizar o combo.");
            }
        }

        // DELETE: api/Combo/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                var combo = await _db.Combos.FindAsync(id);

                if (combo == null)
                    return NotFound("Combo não encontrado.");

                var comboProducts = await _db.ComboProduct
                    .Where(cp => cp.IdCombo == id)
                    .ToListAsync();

                _db.ComboProduct.RemoveRange(comboProducts);
                await _db.SaveChangesAsync();

                _db.Combos.Remove(combo);
                await _db.SaveChangesAsync();

                await transaction.CommitAsync();

                return NoContent();
            }
            catch
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Erro ao excluir o combo.");
            }
        }
    }
}