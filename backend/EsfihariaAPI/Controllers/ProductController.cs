using EsfihariaAPI.Context;
using EsfihariaAPI.DTOs;
using EsfihariaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EsfihariaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ProductController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductListDTO>>> GetAll()
        {
            var products = await _db.Products
                .Select(p => new ProductListDTO
                {
                    Id = p.Id,
                    IdCategory = p.IdCategory,
                    IdStatus = p.IdStatus,
                    Name = p.Name,
                    Price = p.Price,
                    Image = p.Image
                })
                .ToListAsync();

            return Ok(products);
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductListDTO>> GetById(int id)
        {
            var product = await _db.Products
                .Where(p => p.Id == id)
                .Select(p => new ProductListDTO
                {
                    Id = p.Id,
                    IdCategory = p.IdCategory,
                    IdStatus = p.IdStatus,
                    Name = p.Name,
                    Price = p.Price,
                    Image = p.Image
                })
                .FirstOrDefaultAsync();

            if (product == null)
                return NotFound("Produto não encontrado.");

            return Ok(product);
        }

        // POST: api/Product
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateProductDTO dto)
        {
            var product = new Product
            {
                IdCategory = dto.IdCategory,
                IdStatus = dto.IdStatus,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Image = dto.Image
            };

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, dto);
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateProductDTO dto)
        {
            var product = await _db.Products.FindAsync(id);

            if (product == null)
                return NotFound("Produto não encontrado.");

            product.IdCategory = dto.IdCategory;
            product.IdStatus = dto.IdStatus;
            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Image = dto.Image;

            await _db.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var product = await _db.Products.FindAsync(id);

            if (product == null)
                return NotFound("Produto não encontrado.");

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}