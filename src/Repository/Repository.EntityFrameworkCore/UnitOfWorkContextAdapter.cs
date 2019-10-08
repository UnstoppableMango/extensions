using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace UnMango.Extensions.Repository
{
    internal class UnitOfWorkContextAdapter : UnitOfWorkContextAdapter<DbContext>
    {
        public UnitOfWorkContextAdapter(DbContext context)
            : base(context) { }
    }

    internal class UnitOfWorkContextAdapter<T> : IUnitOfWork
        where T : DbContext
    {
        public UnitOfWorkContextAdapter(T context)
        {
            Context = context;
        }

        public T Context { get; }

        public void Dispose() => Context.Dispose();

        public ValueTask DisposeAsync() => Context.DisposeAsync();

        public void SaveChanges() => Context.SaveChanges();

        public ValueTask SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return new ValueTask(Context.SaveChangesAsync(cancellationToken));
        }
    }
}
