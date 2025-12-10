using System.ComponentModel.DataAnnotations;

namespace Ditransa.Domain.Entities.WTSA
{
    /// <summary>
    /// It represents evidence collected during an inspection.
    /// </summary>
    public class Evidence
    {
        [Key]
        public Guid EvidenceId { get; set; }
        public Guid InspectionId { get; set; }
        public string Area { get; set; } = null!;
        public string? Description { get; set; }
        public string? Recommendations { get; set; }
        public string? Legislation { get; set; }
        public Guid? PriorityId { get; set; }
        public Guid? AcpmId { get; set; }

        // Navegación
        public virtual Inspection Inspection { get; set; } = null!;
        public virtual Priority? Priority { get; set; }
        public virtual ACPM? Acpm { get; set; }
    }
}
