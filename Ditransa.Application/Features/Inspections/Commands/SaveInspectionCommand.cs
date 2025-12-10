using Ditransa.Application.Features.Inspections.DTOs;
using Ditransa.Application.Interfaces.Repositories;
using Ditransa.Application.Interfaces.Repositories.Inspections;
using Ditransa.Domain.Entities.WTSA;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ditransa.Application.Features.Inspections.Commands
{
    public record SaveInspectionCommand : IRequest<Inspection>
    {
        public CreateInspectionDto InspectionDto { get; set; } = new CreateInspectionDto();
    }

    internal class SaveInspectionCommandHandler : IRequestHandler<SaveInspectionCommand, Inspection>
    {
        private readonly IInspectionRepository _inspectionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SaveInspectionCommandHandler> _logger;
        public SaveInspectionCommandHandler(IInspectionRepository inspectionRepository, IUnitOfWork unitOfWork, ILogger<SaveInspectionCommandHandler> logger)
        {
            _inspectionRepository = inspectionRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Inspection> Handle(SaveInspectionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var inspectionToSave = new Inspection
                {
                    CompanyId = request.InspectionDto.CompanyId,
                    ConstructionSite = request.InspectionDto.ConstructionSite,
                    UserId = request.InspectionDto.UserId,
                    SSTResponsible = request.InspectionDto.SSTResponsible,
                    InspectionDate = request.InspectionDto.InspectionDate,
                    AnalysisResults = request.InspectionDto.AnalysisResults,
                    Recommendations = request.InspectionDto.Recommendations,
                    Conclusions = request.InspectionDto.Conclusions,
                    Objective = request.InspectionDto.Objective,
                    City = request.InspectionDto.City!,
                    Status = "Ingresado"
                };

                var result = await _inspectionRepository.AddAsync(inspectionToSave);
                await _unitOfWork.Save(cancellationToken);

                if (result == null)
                {
                    _logger.LogWarning("Failed to save inspection for Company ID {CompanyId}", request.InspectionDto.CompanyId);
                    throw new Exception("Failed to save the inspection.");
                }
                _logger.LogInformation("Inspection for Company ID {CompanyId} saved successfully", request.InspectionDto.CompanyId);
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving inspection for Company ID {CompanyId}", request.InspectionDto.CompanyId);
                throw new Exception($"Error: {ex.Message}");
            }
        }
    }
}
