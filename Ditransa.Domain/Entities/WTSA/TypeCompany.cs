using System.ComponentModel.DataAnnotations;

namespace Ditransa.Domain.Entities.WTSA
{
    /// <summary>
    /// It represents a type or category of company within the system.
    /// </summary>
    public class TypeCompany
    {
        [Key]
        public Guid TypeCompanyId { get; set; }
        public string Description { get; set; } = null!;

        // Navegación
        public virtual ICollection<Company> Companies { get; set; } = new List<Company>();
    }
}
