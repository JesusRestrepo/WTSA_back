using System.ComponentModel.DataAnnotations;

namespace Ditransa.Domain.Entities.WTSA
{
    /// <summary>
    /// It represents a type action.
    /// </summary>
    public class ACPM
    {
        [Key]
        public Guid AcpmId { get; set; }
        public string Value { get; set; } = null!;

        // Navegación
        public virtual ICollection<Evidence> Evidences { get; set; } = new List<Evidence>();
    }
}
