using Ditransa.Application.Common.Exceptions;
using Ditransa.Application.DTOs.Inspections.Evidences;
using Ditransa.Application.Features.Inspections.Commands;
using Ditransa.Application.Features.Inspections.DTOs;
using Ditransa.Application.Features.Inspections.Queries;
using Ditransa.Domain.Entities.WTSA;
using Ditransa.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Ditransa.Api.Controllers
{
    [ApiController]
    [EnableRateLimiting("LoginPolicy")]
    [Route("[controller]/[action]")]
    public class InspectionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InspectionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<PaginatedResult<Inspection>> GetInspections([FromQuery] GetInspectionsQuery query)
        {
            var result = (await _mediator.Send(query)).Data;
            return result;
        }

        [HttpGet("{inspectionId:guid}")]
        public async Task<ActionResult<Inspection>> GetInspectionById(Guid inspectionId)
        {
            try
            {
                var query = new GetInspectionByIdQuery { InspectionId = inspectionId };
                var inspection = await _mediator.Send(query);

                return Ok(inspection);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CreateInspectionDto>> SaveInspection([FromBody] CreateInspectionDto form)
        {
            try
            {
                var command = new SaveInspectionCommand { InspectionDto = form };
                var result = await _mediator.Send(command);
                if (result == null)
                {
                    return BadRequest("Failed to save the inspection.");
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }

        }

        [HttpPut("{inspectionId:guid}")]
        public async Task<ActionResult<bool>> UpdateInspection([FromBody] UpdateInspectionDto form, Guid inspectionId)
        {
            try
            {
                var command = new UpdateInspectionCommand { InspectionDto = form, InspectionId = inspectionId };
                var result = await _mediator.Send(command);
                if (result == null)
                {
                    return BadRequest("Failed to update the inspection.");
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<bool>> SaveEvidence([FromBody] CreateEvidenceDto form)
        {
            try
            {
                var command = new SaveEvidencesCommand { Evidence = form };
                var result = await _mediator.Send(command);
                if (result == null)
                {
                    return BadRequest("Failed to save the inspection.");
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{evidenceId:guid}")]
        public async Task<ActionResult<CreateEvidenceDto>> GetEvidenceById(Guid evidenceId)
        {
            try
            {
                var query = new GetEvidenceByIdQuery { EvidenceId = evidenceId };
                var evidence = await _mediator.Send(query);

                return Ok(evidence);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
