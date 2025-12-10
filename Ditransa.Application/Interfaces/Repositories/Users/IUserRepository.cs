using Ditransa.Domain.Entities.WTSA;

namespace Ditransa.Application.Interfaces.Repositories.Users
{
    /// <summary>
    /// Interface for User Repository
    /// </summary>
    public interface IUserRepository : IGenericRepository<User>
    {
        /// <summary>
        /// Method to find a user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<User?> FindByEmailAsync(string email);

    }
}
