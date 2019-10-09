using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace UnMango.Extensions.Repository
{
    public interface IAsyncRepository : IRepository { }

    public interface IAsyncRepository<T> : IAsyncRepository
    {
        ValueTask<T> FindAsync(params object[] keyValues);

        ValueTask<T> FindAsync(object[] keyValues, CancellationToken cancellationToken = default);

        ValueTask<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        ValueTask AddRangeAsync(params T[] entities) => AddRangeAsync((IEnumerable<T>)entities);

        ValueTask AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    }
}
