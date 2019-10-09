using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnMango.Extensions.Repository.CosmosDb
{
    public class Repository<T> : IAsyncRepository<T>
    {
        public IUnitOfWork UnitOfWork { get; }

        public ValueTask<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<T> FindAsync(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public ValueTask<T> FindAsync(object[] keyValues, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
