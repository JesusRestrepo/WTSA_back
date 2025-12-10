using Ditransa.Application.Extensions;
using Ditransa.Application.Interfaces.Repositories.Inspections;
using Ditransa.Domain.Entities.WTSA;
using Ditransa.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ditransa.Application.Features.Inspections.Queries
{
    public record GetInspectionsQuery : IRequest<Result<PaginatedResult<Inspection>>>
    {
        public string? SearchText { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; } = Constants.MAX_PAGE_SIZE;
        public string? SortField { get; set; }
        public string? SortType { get; set; }
    }

    internal class GetInspectionsQueryHandler : IRequestHandler<GetInspectionsQuery, Result<PaginatedResult<Inspection>>>
    {
        private readonly IInspectionRepository _inspectionRepository;
        private readonly ILogger<GetInspectionsQueryHandler> _logger;

        public GetInspectionsQueryHandler(IInspectionRepository inspectionRepository, ILogger<GetInspectionsQueryHandler> logger)
        {
            _inspectionRepository = inspectionRepository;
            _logger = logger;
        }

        public async Task<Result<PaginatedResult<Inspection>>> Handle(GetInspectionsQuery request, CancellationToken cancellationToken)
        {
            var query = _inspectionRepository
                .Entities
                .AsQueryable()
                .Where(i => i.Status != "Aprobado");

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                var search = request.SearchText.Trim();

                query = query.Where(i =>
                    i.Company.Name.Contains(search) ||         
                    i.InspectionDate.ToString().Contains(search) ||         
                    i.City.Contains(search)     
                );
            }

            if (!string.IsNullOrEmpty(request.SortField))
            {
                query = query.OrderBy(request.SortField, request.SortType?.ToLower() == "asc");
            }
            else
            {
                query = query.OrderByDescending(i => i.InspectionDate);
            }

            var paged = await query.ToPaginatedListAsync(request.PageIndex, request.PageSize, cancellationToken);

            var result = new PaginatedResult<Inspection>(
                paged.Succeeded,
                paged.Data.ToList(),
                paged.Messages,
                paged.TotalCount,
                paged.CurrentPage,
                paged.PageSize
            );

            _logger.LogInformation("Retrieved {Count} inspections (Page {PageIndex} of {TotalPages})", result.Data.Count, result.CurrentPage, result.TotalPages);
            return await Result<PaginatedResult<Inspection>>.SuccessAsync(result);
        }
    }
}
