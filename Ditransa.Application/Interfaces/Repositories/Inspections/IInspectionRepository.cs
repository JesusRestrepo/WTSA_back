using Ditransa.Domain.Entities.WTSA;

namespace Ditransa.Application.Interfaces.Repositories.Inspections
{
    /// <summary>
    /// Interface for Inspection Repository
    /// </summary>
    public interface IInspectionRepository : IGenericRepository<Inspection>
    {
        /// <summary>
        /// Update entity inspection by entity in
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateByInspectionIdAsync(Inspection entity);

        /// <summary>
        /// Get entity by id
        /// </summary>
        /// <param name="inspectionId"></param>
        /// <returns></returns>
        Task<Inspection> GetByInspectionIdAsync(Guid inspectionId);
    }
}
