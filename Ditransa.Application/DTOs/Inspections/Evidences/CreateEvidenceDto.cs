namespace Ditransa.Application.DTOs.Inspections.Evidences
{
    public class CreateEvidenceDto
    {
        public Guid InspectionId { get; set; }
        public string Area { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Recommendations { get; set; } = string.Empty;
        public string Legislation { get; set; } = string.Empty;
        public Guid PriorityId { get; set; }
        public Guid ACPMId { get; set; }
    }
}
