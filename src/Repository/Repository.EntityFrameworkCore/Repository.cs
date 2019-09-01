using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using UnMango.Extensions.Repository.EntityFrameworkCore.Internal;

namespace UnMango.Extensions.Repository.EntityFrameworkCore
{
    /// <summary>
    ///     The repository is an implementation of the repository pattern for <typeparamref name="TEntity"/>
    /// </summary>
    /// <typeparam name="TEntity">Type of entity the repository manages</typeparam>
    public abstract class Repository<TEntity> : DbSet<TEntity>, IInternalRepository<TEntity>
        where TEntity : class
    {
        protected Repository([NotNull] DbContext context)
        {
            Context = context;
        }

        public DbContext Context { get; }

        /// <inheritdoc/>
        public int SaveChanges(bool acceptAllChangesOnSuccess = true)
            => Context.SaveChanges(acceptAllChangesOnSuccess);

        /// <inheritdoc/>
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => Context.SaveChangesAsync(cancellationToken);

        /// <inheritdoc/>
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
