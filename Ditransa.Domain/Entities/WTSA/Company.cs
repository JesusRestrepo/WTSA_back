using System.ComponentModel.DataAnnotations;

namespace Ditransa.Domain.Entities.WTSA
{
    /// <summary>
    /// It represents a company within the system.
    /// </summary>
    public class Company
    {
        [Key]
        public Guid CompanyId { get; set; }
        public string Name { get; set; } = null!;
        public string Nit { get; set; } = null!;
        public string? Email { get; set; }
        public Guid TypeCompanyId { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navegación
        public virtual TypeCompany TypeCompany { get; set; } = null!;
        public virtual ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();
        public virtual ICollection<InspectionSchedule> InspectionSchedules { get; set; } = new List<InspectionSchedule>();
    }
}
