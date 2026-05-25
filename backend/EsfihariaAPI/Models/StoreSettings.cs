namespace EsfihariaAPI.Models
{
    public class StoreSettings
    {
        public int Id { get; set; }

        public int IsOpen { get; set; }

        public int EstimatedDeliveryTimeMinutes { get; set; }

        public string PixKey { get; set; } = string.Empty;

        public decimal DeliveryFee { get; set; }

        public decimal MinimumOrderValue { get; set; }

        public string? Phone { get; set; }

        public DateTime UpdateAt { get; set; }
    }
}
