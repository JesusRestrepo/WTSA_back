using Ditransa.Application.Interfaces.Repositories.MenuRol;
using Ditransa.Persistence.Contexts;
using Entity = Ditransa.Domain.Entities.WTSA;

namespace Ditransa.Persistence.Repositories.MenuRol
{
    public class MenuRolRepository : GenericRepository<Entity.MenuRole>, IMenuRolRepository
    {
        public MenuRolRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
