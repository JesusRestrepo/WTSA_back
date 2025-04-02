namespace Ditransa.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class//, IEntity
    {
        IQueryable<T> Entities { get; }

        Task<T> GetByIdAsync(Guid id);

        Task<List<T>> GetAllAsync();

        Task<T> AddAsync(T entity);

        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task DeleteRangeAsync(IEnumerable<T> entities);

        Task<int> Save();
    }
}