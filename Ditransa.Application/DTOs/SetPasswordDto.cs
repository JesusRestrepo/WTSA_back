namespace Ditransa.Application.DTOs
{
    public class SetPasswordDto
    {
        public string Token { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}