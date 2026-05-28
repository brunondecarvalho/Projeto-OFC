using EsfihariaAPI.Context;
using EsfihariaAPI.DTOs;
using EsfihariaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EsfihariaAPI.Controllers
{
    [Route("api/store-settings")]
    [ApiController]
    public class StoreSettingsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public StoreSettingsController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/store-settings
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<StoreSettingsDTO>> Get()
        {
            var settings = await _db.StoreSettings
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (settings == null)
                return NotFound("Configurações da loja não encontradas.");

            return Ok(new StoreSettingsDTO
            {
                IsOpen = settings.IsOpen,
                EstimatedDeliveryTimeMinutes = settings.EstimatedDeliveryTimeMinutes,
                PixKey = settings.PixKey,
                DeliveryFee = settings.DeliveryFee,
                MinimumOrderValue = settings.MinimumOrderValue,
                Phone = settings.Phone,
                UpdateAt = settings.UpdateAt
            });
        }

        // PUT: api/store-settings
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult> Update(
            [FromBody] UpdateStoreSettingsDTO dto)
        {
            var settings = await _db.StoreSettings
                .FirstOrDefaultAsync();

            if (settings == null)
            {
                settings = new StoreSettings();

                _db.StoreSettings.Add(settings);
            }

            if (dto.EstimatedDeliveryTimeMinutes <= 0)
                return BadRequest("O tempo estimado deve ser maior que zero.");

            if (dto.DeliveryFee < 0)
                return BadRequest("A taxa de entrega não pode ser negativa.");

            if (dto.MinimumOrderValue < 0)
                return BadRequest("O pedido mínimo não pode ser negativo.");

            settings.IsOpen = dto.IsOpen;
            settings.EstimatedDeliveryTimeMinutes =
                dto.EstimatedDeliveryTimeMinutes;

            settings.PixKey = dto.PixKey.Trim();
            settings.DeliveryFee = dto.DeliveryFee;
            settings.MinimumOrderValue = dto.MinimumOrderValue;
            settings.Phone = dto.Phone?.Trim();
            settings.UpdateAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/store-settings/is-open
        [AllowAnonymous]
        [HttpGet("is-open")]
        public async Task<ActionResult> IsStoreOpen()
        {
            var settings = await _db.StoreSettings
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (settings == null)
                return NotFound();

            return Ok(new
            {
                settings.IsOpen
            });
        }
    }
}