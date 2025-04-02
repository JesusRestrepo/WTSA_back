using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ditransa.Application.Interfaces.Repositories.Clientes
{
    public interface IUnitOfWorkClientes : IDisposable
    {
        IClientesRepository<T> Repository<T>() where T : class;//BaseEntity;

        Task<int> Save(CancellationToken cancellationToken);

        Task<int> SaveAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys);

        Task Rollback();
    }
}
