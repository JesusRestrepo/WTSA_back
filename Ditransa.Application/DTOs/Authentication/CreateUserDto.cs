namespace Ditransa.Application.DTOs.Authentication
{
    /// <summary>
    /// Data Transfer Object for creating a new user
    /// </summary>
    public class CreateUserDto
    {
        public Guid? UserId { get; set; }
        public string Email { get; set; } = null!;
        public string? Password { get; set; }
        public bool CanLogin { get; set; }
        public Guid RolId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Position { get; set; } = null!;
    }
}
