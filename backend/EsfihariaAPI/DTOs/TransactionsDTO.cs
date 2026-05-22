namespace EsfihariaAPI.DTOs
{
    public class TransactionListDTO
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdOrder { get; set; }
        public int IdStatus { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal TotalValue { get; set; }
        public DateTime CreationDate { get; set; }
    }

    public class TransactionDetailsDTO
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdOrder { get; set; }
        public int IdStatus { get; set; }
        public string GatewayTransactionId { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public int Installments { get; set; }
        public decimal TotalValue { get; set; }
        public string PayloadResponse { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

    public class CreateTransactionDTO
    {
        public int IdUser { get; set; }
        public int IdOrder { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public int Installments { get; set; }
        public string GatewayTransactionId { get; set; } = string.Empty;
        public string PayloadResponse { get; set; } = string.Empty;
    }

    namespace EsfihariaAPI.DTOs
    {
        public class UpdateTransactionStatusDTO
        {
            public int IdStatus { get; set; }
        }
    }
}