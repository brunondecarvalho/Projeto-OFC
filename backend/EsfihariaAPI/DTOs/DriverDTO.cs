namespace EsfihariaAPI.DTOs
{
    public class UpdateDriverDTO
    {
        public string CNH { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
    }

    public class DriverlistDTO
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string CNH { get; set; } = string.Empty;
        public string LicensePlate { get; set;} = string.Empty;
        public int IdStatus { get; set; }

    }
}
