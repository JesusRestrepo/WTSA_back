using System.ComponentModel.DataAnnotations;

namespace Ditransa.Domain.Entities.WTSA
{
    /// <summary>
    /// It represents a role that can be assigned to users within the system.
    /// </summary>
    public class Role
    {
        [Key]
        public Guid RoleId { get; set; }
        public string Description { get; set; } = null!;

        // Navegación
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
