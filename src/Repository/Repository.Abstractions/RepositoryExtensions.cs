using System.Threading;
using System.Threading.Tasks;

namespace UnMango.Extensions.Repository
{
    public static class RepositoryExtensions
    {
        public static void SaveChanges(this IRepository repository)
        {
            repository.UnitOfWork.SaveChanges();
        }

        public static ValueTask SaveChangesAsync(this IRepository repository, CancellationToken cancellationToken = default)
        {
            return repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
