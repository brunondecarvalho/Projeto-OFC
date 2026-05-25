namespace EsfihariaAPI.DTOs
{
    public class CreateOrderItemDTO
    {
        public int IdProduct { get; set; }
        public int Quantity { get; set; }
    }

    public class CreateOrderDTO
    {
        public int IdUser { get; set; }
        public int IdOrderCategory { get; set; }
        public int? IdAddress { get; set; }
        public decimal DeliveryValue { get; set; }
        public decimal DiscountValue { get; set; }
        public int DeliveryTimeMinutes { get; set; }
        public string? Note { get; set; }
        public List<CreateOrderItemDTO> Items { get; set; } = new();
    }

    public class UpdateOrderStatusDTO
    {
        public int IdStatus { get; set; }
    }

    public class OrderItemDTO
    {
        public int IdProduct { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Image { get; set; }
    }

    public class OrderListDTO
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdOrderCategory { get; set; }
        public int? IdAddress { get; set; }
        public int IdStatus { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime Date { get; set; }
        public int ItemsCount { get; set; }
    }

    public class OrderDetailsDTO
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdOrderCategory { get; set; }
        public int? IdAddress { get; set; }
        public int IdStatus { get; set; }
        public decimal SubtotalValue { get; set; }
        public decimal DeliveryValue { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime Date { get; set; }
        public int DeliveryTimeMinutes { get; set; }
        public string? Note { get; set; }
        public List<OrderItemDTO> Items { get; set; } = new();
    }
}