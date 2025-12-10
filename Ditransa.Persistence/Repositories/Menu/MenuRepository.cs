using Ditransa.Application.Interfaces.Repositories.Menu;
using Ditransa.Persistence.Contexts;
using Entity = Ditransa.Domain.Entities.WTSA;

namespace Ditransa.Persistence.Repositories.Menu
{
    public class MenuRepository : GenericRepository<Entity.Menu>, IMenuRepository
    {
        public MenuRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
