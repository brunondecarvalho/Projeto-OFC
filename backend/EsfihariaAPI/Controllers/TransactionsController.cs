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
    public class TransactionsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public TransactionsController(AppDbContext db)
        {
            _db = db;
        }

        //Endpoint para transações ainda não está terminado

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionListDTO>>> GetAll()
        {
            var transactions = await _db.Transactions
                .AsNoTracking()
                .Select(t => new TransactionListDTO
                {
                    Id = t.Id,
                    IdUser = t.IdUser,
                    IdOrder = t.IdOrder,
                    IdStatus = t.IdStatus,
                    PaymentMethod = t.PaymentMethod,
                    TotalValue = t.TotalValue,
                    CreationDate = t.CreationDate
                })
                .ToListAsync();

            return Ok(transactions);
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDetailsDTO>> GetById(int id)
        {
            var transaction = await _db.Transactions
                .AsNoTracking()
                .Where(t => t.Id == id)
                .Select(t => new TransactionDetailsDTO
                {
                    Id = t.Id,
                    IdUser = t.IdUser,
                    IdOrder = t.IdOrder,
                    IdStatus = t.IdStatus,
                    GatewayTransactionId = t.GatewayTransactionId,
                    PaymentMethod = t.PaymentMethod,
                    Installments = t.Installments,
                    TotalValue = t.TotalValue,
                    PayloadResponse = t.PayloadResponse,
                    CreationDate = t.CreationDate,
                    UpdateDate = t.UpdateDate
                })
                .FirstOrDefaultAsync();

            if (transaction == null)
                return NotFound("Transação não encontrada.");

            return Ok(transaction);
        }

        // GET: api/Transactions/order/5
        [HttpGet("order/{idOrder}")]
        public async Task<ActionResult<TransactionDetailsDTO>> GetByOrder(int idOrder)
        {
            var transaction = await _db.Transactions
                .AsNoTracking()
                .Where(t => t.IdOrder == idOrder)
                .Select(t => new TransactionDetailsDTO
                {
                    Id = t.Id,
                    IdUser = t.IdUser,
                    IdOrder = t.IdOrder,
                    IdStatus = t.IdStatus,
                    GatewayTransactionId = t.GatewayTransactionId,
                    PaymentMethod = t.PaymentMethod,
                    Installments = t.Installments,
                    TotalValue = t.TotalValue,
                    PayloadResponse = t.PayloadResponse,
                    CreationDate = t.CreationDate,
                    UpdateDate = t.UpdateDate
                })
                .FirstOrDefaultAsync();

            if (transaction == null)
                return NotFound("Transação não encontrada.");

            return Ok(transaction);
        }

        // POST: api/Transactions
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateTransactionDTO dto)
        {
            var order = await _db.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == dto.IdOrder);

            if (order == null)
                return BadRequest("Pedido não encontrado.");

            if (order.IdUser != dto.IdUser)
                return BadRequest("Pedido não pertence ao usuário.");

            var transactionExists = await _db.Transactions
                .AnyAsync(t => t.GatewayTransactionId == dto.GatewayTransactionId);

            if (transactionExists)
                return Conflict("Transação já registrada.");

            var transaction = new Transactions
            {
                IdUser = dto.IdUser,
                IdOrder = dto.IdOrder,

                // 1 = pendente
                IdStatus = 1,

                GatewayTransactionId = dto.GatewayTransactionId,
                PaymentMethod = dto.PaymentMethod,
                Installments = dto.Installments,

                // SEMPRE usa o valor do pedido
                TotalValue = order.TotalValue,

                PayloadResponse = dto.PayloadResponse,

                CreationDate = DateTime.UtcNow
            };

            _db.Transactions.Add(transaction);
            await _db.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = transaction.Id },
                new
                {
                    transaction.Id,
                    transaction.IdOrder,
                    transaction.TotalValue,
                    transaction.CreationDate
                }
            );
        }

        // PUT: api/Transactions/5/status
        [HttpPut("{id}/status")]
        public async Task<ActionResult> UpdateStatus(
            int id,
            [FromBody] UpdateTransactionStatusDTO dto)
        {
            var transaction = await _db.Transactions.FindAsync(id);

            if (transaction == null)
                return NotFound("Transação não encontrada.");

            transaction.IdStatus = dto.IdStatus;
            transaction.UpdateDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Transactions/webhook
        [HttpPost("webhook")]
        public async Task<ActionResult> MercadoPagoWebhook([FromBody] object payload)
        {
            // Aqui futuramente você valida:
            // - assinatura
            // - origem
            // - evento
            // - consulta API Mercado Pago

            // Exemplo:
            // approved => status pago
            // rejected => status recusado
            // pending => aguardando

            return Ok();
        }
    }
}