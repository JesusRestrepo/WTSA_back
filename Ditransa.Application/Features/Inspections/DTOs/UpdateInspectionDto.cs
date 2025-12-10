namespace Ditransa.Application.Features.Inspections.DTOs
{
    /// <summary>
    /// DTO for updating an existing Inspection
    /// </summary>
    public class UpdateInspectionDto
    {
        public string? AnalysisResults { get; set; }
        public string? Recommendations { get; set; }
        public string? Conclusions { get; set; }
        public string? Status { get; set; }
    }
}
