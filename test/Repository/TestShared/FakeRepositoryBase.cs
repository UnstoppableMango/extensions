using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using KG.Data;

namespace TestShared
{
    public abstract class FakeRepositoryBase : IRepository
    {
        public IEnumerator GetEnumerator() => default;

        public bool SaveChanges() => default;

        public Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
            => default;
    }
}
