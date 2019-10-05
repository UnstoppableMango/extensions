using System.Threading;
using System.Threading.Tasks;

namespace UnMango.Extensions.Repository.EntityFrameworkCore
{
    public static class EFRepositoryExtensions
    {
        public static int SaveChanges<T>(this IEFRepository<T> repository, bool acceptAllChangesOnSuccess)
            where T : class
        {
            return repository.Context.SaveChanges(acceptAllChangesOnSuccess);
        }

        public static Task<int> SaveChangesAsync<T>(
            this IEFRepository<T> repository,
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
            where T : class
        {
            return repository.Context.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
