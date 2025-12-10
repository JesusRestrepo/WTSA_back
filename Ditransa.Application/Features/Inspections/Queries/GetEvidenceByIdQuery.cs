using Ditransa.Application.Common.Exceptions;
using Ditransa.Application.DTOs.Inspections.Evidences;
using Ditransa.Application.Interfaces.Repositories.Evidences;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ditransa.Application.Features.Inspections.Queries
{
    public record GetEvidenceByIdQuery : IRequest<CreateEvidenceDto>
    {
        public Guid EvidenceId { get; set; }
    }

    internal class GetEvidenceByIdQueryHandler : IRequestHandler<GetEvidenceByIdQuery, CreateEvidenceDto>
    {
        private readonly IEvidencesRepository _repository;
        private readonly ILogger<GetEvidenceByIdQueryHandler> _logger;

        public GetEvidenceByIdQueryHandler(IEvidencesRepository evidencesRepository, ILogger<GetEvidenceByIdQueryHandler> logger)
        {
            _repository = evidencesRepository;
            _logger = logger;
        }
        public async Task<CreateEvidenceDto> Handle(GetEvidenceByIdQuery request, CancellationToken cancellationToken)
        {
            var evidence = await _repository.GetByEvidenceIdAsync(request.EvidenceId);
            if (evidence == null)
            {
                _logger.LogWarning("Evidence with ID {EvidenceId} not found", request.EvidenceId);
                throw new NotFoundException($"Inspection not found.");
            }

            var evidenceResult = new CreateEvidenceDto
            {
                InspectionId = evidence.InspectionId,
                Area = evidence.Area,
                Description = evidence.Description!,
                Recommendations = evidence.Recommendations!,
                Legislation = evidence.Legislation!,
                PriorityId = (Guid)evidence.PriorityId!,
                ACPMId = (Guid)evidence.AcpmId!
            };

            _logger.LogInformation("Evidence with ID {EvidenceId} retrieved successfully", request.EvidenceId);
            return evidenceResult;
        }
    }
}