using System.ComponentModel.DataAnnotations;

namespace Ditransa.Domain.Entities.WTSA
{
    /// <summary>
    /// It represents an inspection conducted at a construction site.
    /// </summary>
    public class Inspection
    {
        [Key]
        public Guid InspectionId { get; set; }
        public Guid CompanyId { get; set; }
        public string ConstructionSite { get; set; } = null!;
        public Guid UserId { get; set; }
        public string? SSTResponsible { get; set; }
        public DateTime InspectionDate { get; set; }
        public string? AnalysisResults { get; set; }
        public string? Recommendations { get; set; }
        public string? Conclusions { get; set; }
        public string? Objective { get; set; }
        public Guid ScheduleId { get; set; }
        public string City { get; set; } = null!;
        public string Status { get; set; } = null!;

        // Navegación
        public virtual Company Company { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual InspectionSchedule Schedule { get; set; } = null!;
        public virtual ICollection<Evidence> Evidences { get; set; } = new List<Evidence>();
    }
}
