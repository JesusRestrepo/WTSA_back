using Ditransa.Application.Features.Inspections.DTOs;
using Ditransa.Application.Interfaces.Repositories;
using Ditransa.Application.Interfaces.Repositories.Inspections;
using Ditransa.Domain.Entities.WTSA;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ditransa.Application.Features.Inspections.Commands
{
    public record UpdateInspectionCommand : IRequest<bool>
    {
        public Guid InspectionId { get; set; }
        public UpdateInspectionDto InspectionDto { get; set; } = new UpdateInspectionDto();
    }

    internal class UpdateInspectionCommandHandler : IRequestHandler<UpdateInspectionCommand, bool>
    {
        private readonly IInspectionRepository _inspectionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateInspectionCommandHandler> _logger;

        public UpdateInspectionCommandHandler(IInspectionRepository inspectionRepository, IUnitOfWork unitOfWork, ILogger<UpdateInspectionCommandHandler> logger)
        {
            _inspectionRepository = inspectionRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<bool> Handle(UpdateInspectionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var inspectionExists = await _inspectionRepository.GetByInspectionIdAsync(request.InspectionId);
                if (inspectionExists == null) {
                    _logger.LogWarning("Inspection with ID {InspectionId} not found for update", request.InspectionId);
                    throw new Exception("Inspection not found.");
                }

                var Inspection = new Inspection
                {
                    CompanyId = inspectionExists.CompanyId,
                    ConstructionSite = inspectionExists.ConstructionSite,
                    UserId = inspectionExists.UserId,
                    SSTResponsible = inspectionExists.SSTResponsible,
                    InspectionDate = inspectionExists.InspectionDate,
                    InspectionId = request.InspectionId,
                    AnalysisResults = request.InspectionDto.AnalysisResults,
                    Recommendations = request.InspectionDto.Recommendations,
                    Conclusions = request.InspectionDto.Conclusions,
                    Objective = inspectionExists.Objective,
                    City = inspectionExists.City,
                    Status = request.InspectionDto.Status!
                };

                await _inspectionRepository.UpdateByInspectionIdAsync(Inspection);
                await _unitOfWork.Save(cancellationToken);

                _logger.LogInformation("Inspection with ID {InspectionId} updated successfully", request.InspectionId);
                return true;
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error updating inspection with ID {InspectionId}", request.InspectionId);
                throw new Exception($"Error: {ex.Message}");
            }
            
        }
    }
}