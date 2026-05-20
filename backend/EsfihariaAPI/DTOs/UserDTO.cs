namespace EsfihariaAPI.DTOs
{
    public class UserListDTO
    {
        public int Id { get; set; }
        public int IdRole { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }

    public class UpdateUserDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int IdRole { get; set; }
    }

    public class SignUpDTO
    {
        public int IdRole { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        // Somente para motoristas
        public string? Cnh { get; set; }
        public string? LicensePlate { get; set; }
    }

    public class LoginDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
