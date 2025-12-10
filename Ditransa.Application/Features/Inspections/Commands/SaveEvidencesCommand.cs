using Ditransa.Application.DTOs.Inspections.Evidences;
using Ditransa.Application.Interfaces.Repositories;
using Ditransa.Application.Interfaces.Repositories.Evidences;
using Ditransa.Domain.Entities.WTSA;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ditransa.Application.Features.Inspections.Commands
{
    public record SaveEvidencesCommand : IRequest<bool>
    {
        public CreateEvidenceDto Evidence { get; set; } = new CreateEvidenceDto();
    }

    internal class SaveEvidencesCommandHandler : IRequestHandler<SaveEvidencesCommand, bool>
    {
        private readonly IEvidencesRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SaveEvidencesCommandHandler> _logger;

        public SaveEvidencesCommandHandler(IEvidencesRepository evidencesRepository, IUnitOfWork unitOfWork, ILogger<SaveEvidencesCommandHandler> logger)
        {
            _repository = evidencesRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<bool> Handle(SaveEvidencesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var evidenceToSave = new Evidence
                {
                    InspectionId = request.Evidence.InspectionId,
                    Area = request.Evidence.Area,
                    Description = request.Evidence.Description,
                    Recommendations = request.Evidence.Recommendations,
                    Legislation = request.Evidence.Legislation,
                    PriorityId = request.Evidence.PriorityId,
                    AcpmId = request.Evidence.ACPMId
                };

                var result = await _repository.AddAsync(evidenceToSave);
                await _unitOfWork.Save(cancellationToken);

                if (result != null)
                {
                    _logger.LogInformation("Evidence for Inspection ID {InspectionId} saved successfully", request.Evidence.InspectionId);
                    return true;
                }
                _logger.LogWarning("Failed to save evidence for Inspection ID {InspectionId}", request.Evidence.InspectionId);
                return false;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving evidence for Inspection ID {InspectionId}", request.Evidence.InspectionId);
                throw new Exception($"Error: {ex.Message}");
            }
        }
    }
}
