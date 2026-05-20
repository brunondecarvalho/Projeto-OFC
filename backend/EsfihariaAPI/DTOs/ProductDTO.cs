namespace EsfihariaAPI.DTOs
{
    public class ProductListDTO
    {
        public int Id { get; set; }
        public int IdCategory { get; set; }
        public int IdStatus { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Image { get; set; }
    }

    public class CreateProductDTO
    {
        public int IdCategory { get; set; }
        public int IdStatus { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Image { get; set; }
    }

    public class UpdateProductDTO
    {
        public int IdCategory { get; set; }
        public int IdStatus { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Image { get; set; }
    }
}