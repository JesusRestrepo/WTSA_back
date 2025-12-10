using System.ComponentModel.DataAnnotations;

namespace Ditransa.Domain.Entities.WTSA
{
    /// <summary>
    /// It represents a user within the system.
    /// </summary>
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        public string Email { get; set; } = null!;
        public string Salt { get; set; } = null!;
        public string HashedPwd { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public bool CanLogin { get; set; }
        public Guid RoleId { get; set; }
        public string? Position { get; set; }

        // Navegación
        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();
    }
}
