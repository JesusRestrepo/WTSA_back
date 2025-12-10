using System.ComponentModel.DataAnnotations;

namespace Ditransa.Domain.Entities.WTSA
{
    /// <summary>
    /// It represents the association between menus and user roles.
    /// </summary>
    public class MenuRole
    {
        [Key]
        public Guid MenuRolId { get; set; }
        public Guid RoleId { get; set; }
        public Guid MenuId { get; set; }

        public virtual Menu Menu { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
    }
}
