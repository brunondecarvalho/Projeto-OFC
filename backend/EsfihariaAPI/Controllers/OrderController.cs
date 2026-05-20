using EsfihariaAPI.Context;
using EsfihariaAPI.DTOs;
using EsfihariaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EsfihariaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _db;

        public OrderController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderListDTO>>> GetAll()
        {
            var orders = await _db.Orders
                .AsNoTracking()
                .Select(o => new OrderListDTO
                {
                    Id = o.Id,
                    IdUser = o.IdUser,
                    IdOrderCategory = o.IdOrderCategory,
                    IdAddress = o.IdAddress,
                    IdStatus = o.IdStatus,
                    TotalValue = o.TotalValue,
                    Date = o.Date,
                    ItemsCount = o.Orderproducts.Count
                })
                .ToListAsync();

            return Ok(orders);
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetailsDTO>> GetById(int id)
        {
            var order = await _db.Orders
                .AsNoTracking()
                .Where(o => o.Id == id)
                .Select(o => new OrderDetailsDTO
                {
                    Id = o.Id,
                    IdUser = o.IdUser,
                    IdOrderCategory = o.IdOrderCategory,
                    IdAddress = o.IdAddress,
                    IdStatus = o.IdStatus,
                    SubtotalValue = o.SubtotalValue,
                    DeliveryValue = o.DeliveryValue,
                    DiscountValue = o.DiscountValue,
                    TotalValue = o.TotalValue,
                    Date = o.Date,
                    DeliveryTime = o.DeliveryTime,
                    Note = o.Note,
                    Items = o.Orderproducts.Select(op => new OrderItemDTO
                    {
                        IdProduct = op.IdProduct,
                        ProductName = op.IdProductNavigation.Name,
                        Quantity = op.Quantity,
                        UnitPrice = op.IdProductNavigation.Price,
                        TotalPrice = op.IdProductNavigation.Price * op.Quantity,
                        Image = op.IdProductNavigation.Image
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (order == null)
                return NotFound("Pedido não encontrado.");

            return Ok(order);
        }

        // GET: api/Order/user/5
        [HttpGet("user/{idUser}")]
        public async Task<ActionResult<IEnumerable<OrderListDTO>>> GetByUser(int idUser)
        {
            var orders = await _db.Orders
                .AsNoTracking()
                .Where(o => o.IdUser == idUser)
                .Select(o => new OrderListDTO
                {
                    Id = o.Id,
                    IdUser = o.IdUser,
                    IdOrderCategory = o.IdOrderCategory,
                    IdAddress = o.IdAddress,
                    IdStatus = o.IdStatus,
                    TotalValue = o.TotalValue,
                    Date = o.Date,
                    ItemsCount = o.Orderproducts.Count
                })
                .ToListAsync();

            return Ok(orders);
        }

        // GET: api/Order/status/1
        [HttpGet("status/{idStatus}")]
        public async Task<ActionResult<IEnumerable<OrderListDTO>>> GetByStatus(int idStatus)
        {
            var orders = await _db.Orders
                .AsNoTracking()
                .Where(o => o.IdStatus == idStatus)
                .Select(o => new OrderListDTO
                {
                    Id = o.Id,
                    IdUser = o.IdUser,
                    IdOrderCategory = o.IdOrderCategory,
                    IdAddress = o.IdAddress,
                    IdStatus = o.IdStatus,
                    TotalValue = o.TotalValue,
                    Date = o.Date,
                    ItemsCount = o.Orderproducts.Count
                })
                .ToListAsync();

            return Ok(orders);
        }

        // POST: api/Order
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateOrderDTO dto)
        {
            if (dto.Items == null || dto.Items.Count == 0)
                return BadRequest("Adicione ao menos um produto ao pedido.");

            var userExists = await _db.Users.AnyAsync(u => u.Id == dto.IdUser);
            if (!userExists)
                return BadRequest("Usuário não encontrado.");

            var categoryExists = await _db.Ordercategories.AnyAsync(c => c.Id == dto.IdOrderCategory);
            if (!categoryExists)
                return BadRequest("Categoria do pedido inválida.");

            if (dto.IdAddress.HasValue)
            {
                var addressExists = await _db.Addresses.AnyAsync(a =>
                    a.Id == dto.IdAddress.Value && a.IdUser == dto.IdUser);

                if (!addressExists)
                    return BadRequest("Endereço inválido para o usuário informado.");
            }

            if (dto.Items.Any(i => i.Quantity <= 0))
                return BadRequest("A quantidade de cada produto deve ser maior que zero.");

            var productIds = dto.Items.Select(i => i.IdProduct).Distinct().ToList();
            var products = await _db.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            if (products.Count != productIds.Count)
                return BadRequest("Um ou mais produtos informados não existem.");

            var productMap = products.ToDictionary(p => p.Id);

            decimal subtotal = 0m;
            foreach (var item in dto.Items)
            {
                var product = productMap[item.IdProduct];
                subtotal += product.Price * item.Quantity;
            }

            var total = subtotal + dto.DeliveryValue - dto.DiscountValue;
            if (total < 0)
                return BadRequest("O valor total do pedido não pode ser negativo.");

            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                var order = new Order
                {
                    IdUser = dto.IdUser,
                    IdOrderCategory = dto.IdOrderCategory,
                    IdAddress = dto.IdAddress,
                    IdStatus = 1, // Pendente, ajuste conforme sua tabela Status
                    SubtotalValue = subtotal,
                    DeliveryValue = dto.DeliveryValue,
                    DiscountValue = dto.DiscountValue,
                    TotalValue = total,
                    Date = DateTime.UtcNow,
                    DeliveryTime = dto.DeliveryTime,
                    Note = dto.Note
                };

                _db.Orders.Add(order);
                await _db.SaveChangesAsync();

                var orderProducts = dto.Items.Select(item => new Orderproduct
                {
                    IdOrder = order.Id,
                    IdProduct = item.IdProduct,
                    Quantity = item.Quantity
                }).ToList();

                _db.Orderproducts.AddRange(orderProducts);
                await _db.SaveChangesAsync();

                await transaction.CommitAsync();

                return CreatedAtAction(nameof(GetById), new { id = order.Id }, new
                {
                    order.Id,
                    order.IdUser,
                    order.IdOrderCategory,
                    order.IdAddress,
                    order.IdStatus,
                    order.SubtotalValue,
                    order.DeliveryValue,
                    order.DiscountValue,
                    order.TotalValue,
                    order.Date,
                    order.DeliveryTime,
                    order.Note
                });
            }
            catch
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Erro ao criar o pedido.");
            }
        }

        // PUT: api/Order/5/status
        [HttpPut("{id}/status")]
        public async Task<ActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusDTO dto)
        {
            var order = await _db.Orders.FindAsync(id);

            if (order == null)
                return NotFound("Pedido não encontrado.");

            order.IdStatus = dto.IdStatus;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Cancel(int id)
        {
            var order = await _db.Orders.FindAsync(id);

            if (order == null)
                return NotFound("Pedido não encontrado.");

            order.IdStatus = 4; // Cancelado, ajuste conforme sua tabela Status
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}