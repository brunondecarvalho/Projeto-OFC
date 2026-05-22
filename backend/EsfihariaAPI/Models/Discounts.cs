using System.Security.Principal;

namespace EsfihariaAPI.Models
{
    public class Discounts
    {
        public int Id { get; set; }
        public string CouponCode { get; set; } = string.Empty;
        public int IdDiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
