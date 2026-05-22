namespace EsfihariaAPI.Models
{
    public class Combo
    {
        public int Id { get; set; }
        public int IdStatus { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
