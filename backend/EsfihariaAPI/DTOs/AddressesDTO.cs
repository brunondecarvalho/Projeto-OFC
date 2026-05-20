namespace EsfihariaAPI.DTOs
{
    public class AddressListDTO
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string Address { get; set; } = string.Empty;
        public int Number { get; set; }
        public string Neighborhood { get; set; } = string.Empty;
        public int Cep { get; set; }
        public string? Complement { get; set; }
        public string? Landmark { get; set; }
    }

    public class CreateAddressDTO
    {
        public int IdUser { get; set; }
        public string Address { get; set; } = string.Empty;
        public int Number { get; set; }
        public string Neighborhood { get; set; } = string.Empty;
        public int Cep { get; set; }
        public string? Complement { get; set; }
        public string? Landmark { get; set; }
    }

    public class UpdateAddressDTO
    {
        public string Address { get; set; } = string.Empty;
        public int Number { get; set; }
        public string Neighborhood { get; set; } = string.Empty;
        public int Cep { get; set; }
        public string? Complement { get; set; }
        public string? Landmark { get; set; }
    }
}