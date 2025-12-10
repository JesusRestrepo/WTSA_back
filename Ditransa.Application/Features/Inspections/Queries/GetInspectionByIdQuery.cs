using Ditransa.Application.Common.Exceptions;
using Ditransa.Application.Interfaces.Repositories.Inspections;
using Ditransa.Domain.Entities.WTSA;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ditransa.Application.Features.Inspections.Queries
{
    public record GetInspectionByIdQuery : IRequest<Inspection>
    {
        public Guid InspectionId { get; set; }
    }

    internal class GetInspectionByIdQueryHandler : IRequestHandler<GetInspectionByIdQuery, Inspection>
    {
        private readonly IInspectionRepository _inspectionRepository;
        private readonly ILogger<GetInspectionByIdQueryHandler> _logger;

        public GetInspectionByIdQueryHandler(IInspectionRepository inspectionRepository, ILogger<GetInspectionByIdQueryHandler> logger)
        {
            _inspectionRepository = inspectionRepository;
            _logger = logger;
        }
        public async Task<Inspection> Handle(GetInspectionByIdQuery request, CancellationToken cancellationToken)
        {
            var inspection = await _inspectionRepository.GetByInspectionIdAsync(request.InspectionId);

            if (inspection == null)
            {
                _logger.LogWarning("Inspection with ID {InspectionId} not found", request.InspectionId);
                throw new NotFoundException($"Inspection not found.");
            }
            _logger.LogInformation("Inspection with ID {InspectionId} retrieved successfully", request.InspectionId);
            return inspection;
        }
    }
}
