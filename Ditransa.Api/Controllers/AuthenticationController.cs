using Ditransa.Application.DTOs;
using Ditransa.Application.Features.Authentication.Commands;
using Ditransa.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Ditransa.Api.Controllers
{
    [ApiController]
    [EnableRateLimiting("LoginPolicy")]
    [Route("[controller]/[action]")]

    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Result<bool>>> RegisterUser([FromBody] CreateUserCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var errorResult = Result<bool>.Failure(ex.Message);
                return BadRequest(errorResult);
            }
        }

        [HttpPost]
        public async Task<ActionResult<LoginResultDto>> Login([FromBody] LoginUserCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var errorResult = Result<LoginResultDto>.Failure(ex.Message);
                return BadRequest(errorResult);
            }
        }
    }
}
