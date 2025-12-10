namespace Ditransa.Application.DTOs.Menu
{
    /// <summary>
    /// Data Transfer Object for Menu
    /// </summary>
    public class MenuDto
    {
        public Guid MenuId { get; set; }
        public string Type { get; set; } = null!;
        public string Value { get; set; } = null!;
        public bool Active { get; set; }
        public string? Link { get; set; }
        public string? Parent { get; set; }
    }
}
