using System.ComponentModel.DataAnnotations;

namespace Ditransa.Domain.Entities.WTSA
{
    /// <summary>
    /// It represents a priority level for evidences.
    /// </summary>
    public class Priority
    {
        [Key]
        public Guid PriorityId { get; set; }
        public string Value { get; set; } = null!;

        // Navegación
        public virtual ICollection<Evidence> Evidences { get; set; } = new List<Evidence>();
    }
}