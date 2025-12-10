namespace Ditransa.Application.DTOs.Users
{
    /// <summary>
    /// Data Transfer Object for User
    /// </summary>
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public bool CanLogin { get; set; }
        public Guid RoleId { get; set; }
        public string? Position { get; set; }

        // Opcional, si quieres mostrar el nombre del rol
        public string? RoleDescription { get; set; }
    }
}
