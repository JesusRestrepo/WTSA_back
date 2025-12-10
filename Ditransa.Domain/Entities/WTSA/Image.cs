using System.ComponentModel.DataAnnotations;

namespace Ditransa.Domain.Entities.WTSA
{
    /// <summary>
    /// It represents an image associated with evidences.
    /// </summary>
    public class Image
    {
        [Key]
        public Guid ImageId { get; set; }
        public byte[]? ImageData { get; set; }  // mapea a VARBINARY(MAX)
        public DateTime Date { get; set; }
        public string? Location { get; set; }

        // Navegación
        public virtual ICollection<Evidence> Evidences { get; set; } = new List<Evidence>();
    }
}
