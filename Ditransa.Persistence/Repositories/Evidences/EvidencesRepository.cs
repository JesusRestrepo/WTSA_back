using Ditransa.Application.Interfaces.Repositories.Evidences;
using Ditransa.Domain.Entities.WTSA;
using Ditransa.Persistence.Contexts;

namespace Ditransa.Persistence.Repositories.Evidences
{
    public class EvidencesRepository : GenericRepository<Evidence>, IEvidencesRepository
    {
        public EvidencesRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<Evidence> GetByEvidenceIdAsync(Guid EvidenceId)
        {
            return await _dbContext.Set<Evidence>().FindAsync(EvidenceId);
        }
    }
}
