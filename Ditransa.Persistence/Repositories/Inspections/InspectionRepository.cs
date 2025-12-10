using Ditransa.Application.Interfaces.Repositories.Inspections;
using Ditransa.Domain.Entities.WTSA;
using Ditransa.Persistence.Contexts;

namespace Ditransa.Persistence.Repositories.Inspections
{
    public class InspectionRepository : GenericRepository<Inspection>, IInspectionRepository
    {
        public InspectionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public Task UpdateByInspectionIdAsync(Inspection entity)
        {
            var property = entity.GetType().GetProperty("InspectionId");
            object id = property.GetValue(entity);
            Inspection exist = _dbContext.Set<Inspection>().Find(id);
            _dbContext.Entry(exist).CurrentValues.SetValues(entity);
            return Task.CompletedTask;
        }

        public async Task<Inspection> GetByInspectionIdAsync(Guid inspectionId)
        {
            return await _dbContext.Set<Inspection>().FindAsync(inspectionId);
        }
    }
}
