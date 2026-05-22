namespace EsfihariaAPI.DTOs
{
    namespace EsfihariaAPI.DTOs
    {
        public class ComboItemDTO
        {
            public int IdProduct { get; set; }
            public int Quantity { get; set; }
        }

        public class CreateComboDTO
        {
            public int IdStatus { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public List<ComboItemDTO> Products { get; set; } = new();
        }

        public class UpdateComboDTO
        {
            public int IdStatus { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public List<ComboItemDTO> Products { get; set; } = new();
        }

        public class ComboListDTO
        {
            public int Id { get; set; }
            public int IdStatus { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public int ItemsCount { get; set; }
        }

        public class ComboProductDTO
        {
            public int IdProduct { get; set; }
            public string ProductName { get; set; } = string.Empty;
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal TotalPrice { get; set; }
        }

        public class ComboDetailsDTO
        {
            public int Id { get; set; }
            public int IdStatus { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public List<ComboProductDTO> Products { get; set; } = new();
        }
    }
}
