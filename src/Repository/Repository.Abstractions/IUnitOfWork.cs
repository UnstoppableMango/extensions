using System;
using System.Threading;
using System.Threading.Tasks;

namespace UnMango.Extensions.Repository
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        void SaveChanges();

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
