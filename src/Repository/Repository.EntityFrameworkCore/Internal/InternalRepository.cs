using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace UnMango.Extensions.Repository.EntityFrameworkCore.Internal
{
    internal class InternalRepository<TEntity> : InternalDbSet<TEntity>, IInternalRepository<TEntity>
        where TEntity : class
    {
        public InternalRepository([NotNull] DbContext context)
            : base(context)
        {
            Context = Check.NotNull(context, nameof(context));
        }

        public DbContext Context { get; }

        public int SaveChanges(bool acceptAllChangesOnSuccess = true)
            => Context.SaveChanges(acceptAllChangesOnSuccess);

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => Context.SaveChangesAsync(cancellationToken);

        public Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
            => Context.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        #region IRepository Implementation

        void IRepository<TEntity>.Add(TEntity entity) => base.Add(entity);

        Task IRepository<TEntity>.AddAsync(TEntity entity, CancellationToken cancellationToken)
            => base.AddAsync(entity, cancellationToken);

        void IRepository<TEntity>.Remove(TEntity entity) => base.Remove(entity);

        void IRepository<TEntity>.Update(TEntity entity) => base.Update(entity);

        bool IRepository.SaveChanges() => Context == null || Context.SaveChanges() >= 0;

        async Task<bool> IRepository.SaveChangesAsync(CancellationToken cancellationToken)
            => Context == null || await Context.SaveChangesAsync(cancellationToken) >= 0;

        #endregion
    }
}
