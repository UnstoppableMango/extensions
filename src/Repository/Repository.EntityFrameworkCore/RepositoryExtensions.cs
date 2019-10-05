using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace UnMango.Extensions.Repository
{
    // TODO: Rethink whether DbContext should be the provider for the repository
    public static class RepositoryExtensions
    {
        public static int SaveChanges<T>(this IRepository<T, DbContext> repository, bool acceptAllChangesOnSuccess)
        {
            return repository.Provider.SaveChanges(acceptAllChangesOnSuccess);
        }

        public static Task<int> SaveChangesAsync<T>(
            this IRepository<T, DbContext> repository,
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            return repository.Provider.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
