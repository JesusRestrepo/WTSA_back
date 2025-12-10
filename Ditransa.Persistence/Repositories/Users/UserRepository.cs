using Ditransa.Application.Interfaces.Repositories.Users;
using Ditransa.Domain.Entities.WTSA;
using Ditransa.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Ditransa.Persistence.Repositories.Users
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            var user = await _dbContext.User.Include(u => u.Role).Where(u => u.Email.ToLower().Equals(email.ToLower())).FirstOrDefaultAsync();
            return user;
        }
    }
}
