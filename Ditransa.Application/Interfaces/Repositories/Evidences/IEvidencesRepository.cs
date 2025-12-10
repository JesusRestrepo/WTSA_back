using Ditransa.Domain.Entities.WTSA;

namespace Ditransa.Application.Interfaces.Repositories.Evidences
{
    /// <summary>
    /// Interface for evidence entity in database.
    /// </summary>
    public interface IEvidencesRepository : IGenericRepository<Evidence>
    {
        /// <summary>
        /// Get evidence by its unique identifier.
        /// </summary>
        /// <param name="EvidenceId"></param>
        /// <returns></returns>
        Task<Evidence> GetByEvidenceIdAsync(Guid EvidenceId);
    }
}
