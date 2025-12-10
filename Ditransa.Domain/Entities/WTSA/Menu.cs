using System.ComponentModel.DataAnnotations;

namespace Ditransa.Domain.Entities.WTSA
{
    /// <summary>
    /// It represents a menu item within the system.
    /// </summary>
    public class Menu
    {
        [Key]
        public Guid MenuId { get; set; }
        public string Type { get; set; } = null!;
        public string Value { get; set; } = null!;
        public bool Active { get; set; }
        public string? Link { get; set; }
        public string? Parent { get; set; }

        // Navegación
        public virtual ICollection<MenuRole> MenuRoles { get; set; } = new List<MenuRole>();
    }
}
