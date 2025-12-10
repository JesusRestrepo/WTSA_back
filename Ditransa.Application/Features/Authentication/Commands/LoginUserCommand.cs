using Ditransa.Application.DTOs.Authentication;
using Ditransa.Application.Interfaces.Authentication;
using Ditransa.Shared;
using MediatR;

namespace Ditransa.Application.Features.Authentication.Commands
{
    public record LoginUserCommand : IRequest<Result<LoginResultDto>>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    internal class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<LoginResultDto>>
    {
        private readonly IAuthenticationService _authenticationService;

        public LoginUserCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<Result<LoginResultDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var loginResult = await _authenticationService.Login(request.Email, request.Password, cancellationToken);

            // Aquí ENVUELVES el dto dentro de tu Result<T>
            return await Result<LoginResultDto>.SuccessAsync(loginResult);

            // Si tu LoginResultDto tiene algo como loginResult.Success,
            // podrías hacer algo así:
            /*
            if (loginResult.Success)
                return await Result<LoginResultDto>.SuccessAsync(loginResult);
            else
                return await Result<LoginResultDto>.FailureAsync(loginResult);
            */
        }
    }
}
