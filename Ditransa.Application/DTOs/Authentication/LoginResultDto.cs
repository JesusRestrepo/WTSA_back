using Ditransa.Application.DTOs.Menu;
using Ditransa.Application.DTOs.Users;

namespace Ditransa.Application.DTOs.Authentication
{
    /// <summary>
    /// Data Transfer Object for login result
    /// </summary>
    public class LoginResultDto
    {
        public bool UserFound { get; set; }
        public bool ValidCredentials { get; set; }
        public bool Success { get; set; }

        public string? Email { get; set; }
        public string? Token { get; set; }
        public UserDto? User { get; set; }

        public List<MenuDto> Menus { get; set; } = new();
    }
}
