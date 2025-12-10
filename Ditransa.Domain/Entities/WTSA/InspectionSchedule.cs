using System.ComponentModel.DataAnnotations;

namespace Ditransa.Domain.Entities.WTSA
{
    /// <summary>
    /// It represents an inspection schedule for a company.
    /// </summary>
    public class InspectionSchedule
    {
        [Key]
        public Guid ScheduleId { get; set; }
        public Guid CompanyId { get; set; }
        public DateTime Date { get; set; }

        // Navegación
        public virtual Company Company { get; set; } = null!;
        public virtual ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();
    }
}
