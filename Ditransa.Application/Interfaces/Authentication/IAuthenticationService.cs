using Ditransa.Application.DTOs.Authentication;
using Ditransa.Domain.Entities.WTSA;

namespace Ditransa.Application.Interfaces.Authentication
{
    /// <summary>
    /// Interface for Authentication Service
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Method for register a new user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="canLogin"></param>
        /// <returns></returns>
        Task<User> Register(CreateUserDto user, string password, bool canLogin);

        /// <summary>
        /// Method for login a user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<LoginResultDto> Login(string email, string password, CancellationToken cancellationToken = default);

    }
}
