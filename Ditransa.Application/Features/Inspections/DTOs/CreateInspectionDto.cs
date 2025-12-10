namespace Ditransa.Application.Features.Inspections.DTOs
{
    /// <summary>
    /// DTO for creating a new Inspection
    /// </summary>
    public class CreateInspectionDto
    {
        public Guid CompanyId { get; set; }
        public string ConstructionSite { get; set; } = null!;
        public Guid UserId { get; set; }
        public string SSTResponsible { get; set; } = null!;
        public DateTime InspectionDate { get; set; }

        public string? AnalysisResults { get; set; }
        public string? Recommendations { get; set; }
        public string? Conclusions { get; set; }
        public string? Objective { get; set; }
        public string? City { get; set; }
    }
}
