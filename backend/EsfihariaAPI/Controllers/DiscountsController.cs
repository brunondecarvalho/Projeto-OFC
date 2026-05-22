using EsfihariaAPI.Context;
using EsfihariaAPI.DTOs;
using EsfihariaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EsfihariaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public DiscountsController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/Discounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiscountListDTO>>> GetAll()
        {
            var discounts = await _db.Discounts
                .AsNoTracking()
                .Select(d => new DiscountListDTO
                {
                    Id = d.Id,
                    CouponCode = d.CouponCode,
                    IdDiscountType = d.IdDiscountType,
                    DiscountValue = d.DiscountValue,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate
                })
                .ToListAsync();

            return Ok(discounts);
        }

        // GET: api/Discounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DiscountListDTO>> GetById(int id)
        {
            var discount = await _db.Discounts
                .AsNoTracking()
                .Where(d => d.Id == id)
                .Select(d => new DiscountListDTO
                {
                    Id = d.Id,
                    CouponCode = d.CouponCode,
                    IdDiscountType = d.IdDiscountType,
                    DiscountValue = d.DiscountValue,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate
                })
                .FirstOrDefaultAsync();

            if (discount == null)
                return NotFound("Cupom não encontrado.");

            return Ok(discount);
        }

        // GET: api/Discounts/coupon/BLACKFRIDAY
        [HttpGet("coupon/{coupon}")]
        public async Task<ActionResult<DiscountListDTO>> GetByCoupon(string coupon)
        {
            var discount = await _db.Discounts
                .AsNoTracking()
                .Where(d => d.CouponCode == coupon)
                .Select(d => new DiscountListDTO
                {
                    Id = d.Id,
                    CouponCode = d.CouponCode,
                    IdDiscountType = d.IdDiscountType,
                    DiscountValue = d.DiscountValue,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate
                })
                .FirstOrDefaultAsync();

            if (discount == null)
                return NotFound("Cupom não encontrado.");

            return Ok(discount);
        }

        // POST: api/Discounts
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateDiscountDTO dto)
        {
            var couponExists = await _db.Discounts
                .AnyAsync(d => d.CouponCode == dto.CouponCode);

            if (couponExists)
                return Conflict("Já existe um cupom com esse código.");

            if (dto.EndDate <= dto.StartDate)
                return BadRequest("A data final deve ser maior que a data inicial.");

            if (dto.DiscountValue <= 0)
                return BadRequest("O desconto deve ser maior que zero.");

            var discount = new Discounts
            {
                CouponCode = dto.CouponCode,
                IdDiscountType = dto.IdDiscountCategory,
                DiscountValue = dto.DiscountValue,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };

            _db.Discounts.Add(discount);
            await _db.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = discount.Id },
                new DiscountListDTO
                {
                    Id = discount.Id,
                    CouponCode = discount.CouponCode,
                    IdDiscountType = discount.IdDiscountType,
                    DiscountValue = discount.DiscountValue,
                    StartDate = discount.StartDate,
                    EndDate = discount.EndDate
                }
            );
        }

        // PUT: api/Discounts/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateDiscountDTO dto)
        {
            var discount = await _db.Discounts.FindAsync(id);

            if (discount == null)
                return NotFound("Cupom não encontrado.");

            var couponExists = await _db.Discounts.AnyAsync(d =>
                d.CouponCode == dto.CouponCode &&
                d.Id != id);

            if (couponExists)
                return Conflict("Já existe outro cupom com esse código.");

            if (dto.EndDate <= dto.StartDate)
                return BadRequest("A data final deve ser maior que a data inicial.");

            if (dto.DiscountValue <= 0)
                return BadRequest("O desconto deve ser maior que zero.");

            discount.CouponCode = dto.CouponCode;
            discount.IdDiscountType = dto.IdDiscountCategory;
            discount.DiscountValue = dto.DiscountValue;
            discount.StartDate = dto.StartDate;
            discount.EndDate = dto.EndDate;

            await _db.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Discounts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var discount = await _db.Discounts.FindAsync(id);

            if (discount == null)
                return NotFound("Cupom não encontrado.");

            _db.Discounts.Remove(discount);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}