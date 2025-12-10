using Ditransa.Application.DTOs.Authentication;
using Ditransa.Application.Interfaces.Authentication;
using Ditransa.Shared;
using MediatR;

namespace Ditransa.Application.Features.Authentication.Commands
{
    public record CreateUserCommand : IRequest<Result<bool>>
    {
        public CreateUserDto? User { get; set; }
    }

    internal class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<bool>>
    {
        private readonly IAuthenticationService _authenticationService;

        public CreateUserCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        public async Task<Result<bool>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new CreateUserDto
            {
                Email = request.User!.Email,
                CreatedAt = request.User.CreatedAt,
                RolId = request.User!.RolId,
                Position = request.User.Position
            };

            await _authenticationService.Register(user, request.User.Password!, true);
            return await Result<bool>.SuccessAsync(true);
        }
    }
}
