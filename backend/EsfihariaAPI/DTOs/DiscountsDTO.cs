namespace EsfihariaAPI.DTOs
{
    public class DiscountListDTO
    {
        public int Id { get; set; }
        public string CouponCode { get; set; } = string.Empty;
        public int IdDiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class CreateDiscountDTO
    {
        public string CouponCode { get; set; } = string.Empty;
        public int IdDiscountCategory { get; set; }
        public decimal DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class UpdateDiscountDTO
    {
        public string CouponCode { get; set; } = string.Empty;
        public int IdDiscountCategory { get; set; }
        public decimal DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
