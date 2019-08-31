using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace UnMango.Extensions.Repository
{
    /// <summary>
    ///     A unit of work represents a single transaction against a data source and can be used
    ///     to query and save instances of entities. The <see cref="UnitOfWorkBase"/> instance should
    ///     be disposed when the transaction is finished
    /// </summary>
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        private bool _configured;

        /// <summary>
        ///     Repositories associated with this unit of work's scope
        /// </summary>
        protected readonly ICollection<IRepository> Repositories;

        /// <summary>
        ///     Initializes a new instance of <see cref="UnitOfWorkBase"/> with no repositories.
        /// </summary>
        protected UnitOfWorkBase()
        {
            Repositories = new List<IRepository>();
        }

        /// <summary>
        ///     Initializes the concrete class with the specified repositories.
        ///     Primarily used with dependency injection.
        /// </summary>
        /// <param name="repositories"> Repositories to use. </param>
        protected UnitOfWorkBase([NotNull] IEnumerable<IRepository> repositories)
        {
            Repositories = Check.NotNull(repositories, nameof(repositories)).ToList();
        }

        /// <summary>
        ///     Adds the repository to unit of work scope.
        ///     This method supports the service locator anti-pattern. Avoid it when possible.
        /// </summary>
        /// <param name="repository"> Repository to add. </param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="repository"/> does not implement <see cref="IRepository{TEntity}"/>
        /// or has already been added to the <see cref="IUnitOfWork"/>.
        /// </exception>
        [UsedImplicitly]
        protected internal void Add([NotNull] IRepository repository)
        {
            Check.NotNull(repository, nameof(repository));

            if (!RepoUtility.IsGenericRepositoryType(repository.GetType()))
                throw new ArgumentException($"Repository does not implement {typeof(IRepository<>)}", nameof(repository));

            var entityType = repository.EntityType();

            if (Repositories.Any(x => x.EntityType() == entityType))
                throw new ArgumentException("Can\'t add duplicate repository", nameof(repository));

            Repositories.Add(repository);
        }

        /// <summary>
        ///     Adds the repository to unit of work scope.
        ///     This method supports the service locator anti-pattern. Avoid it when possible.
        /// </summary>
        /// <typeparam name="TEntity"> Entity type of the repository to add. </typeparam>
        /// <param name="repository"> Repository to add. </param>
        protected internal void Add<TEntity>([NotNull] IRepository<TEntity> repository)
            where TEntity : class
        {
            Check.NotNull(repository, nameof(repository));

            if (Repositories.OfType<TEntity>().Any())
                throw new InvalidOperationException("Can\'t add duplicate repository");

            Repositories.Add(repository);
        }

        /// <summary>
        ///     Service locator (anti)pattern to retrieve the repository for <typeparamref name="TEntity"/>.
        ///     This is a convenience method. Avoid using it when possible.
        /// </summary>
        /// <typeparam name="TEntity"> Entity type of the repository to retrieve. </typeparam>
        /// <returns> Repository for <typeparamref name="TEntity"/> </returns>
        protected internal IRepository<TEntity>? Repository<TEntity>()
            where TEntity : class
            => (IRepository<TEntity>)Repository(typeof(TEntity));

        /// <summary>
        ///     Service locator (anti)pattern to retrieve the repository for <paramref name="entityType"/>.
        ///     This is a convenience method. Avoid using it when possible.
        /// </summary>
        /// <param name="entityType"> Entity type of the repository to retrieve. </param>
        /// <returns> Repository for <paramref name="entityType"/> </returns>
        protected internal IRepository? Repository(Type entityType)
        {
            InternalConfigure();

            var repoType = typeof(IRepository<>).MakeGenericType(entityType);
            return Repositories?
                .Where(repoType.IsInstanceOfType)
                .SingleOrDefault();
        }

        /// <summary>
        ///     Persists changes on all configured repositories.
        /// </summary>
        /// <returns> Whether any repository failed to persist changes or not. </returns>
        public virtual bool SaveChanges()
        {
            InternalConfigure();

            return Repositories.Aggregate(true, (current, repository) => current & repository.SaveChanges());
        }

        /// <summary>
        ///     Persists changes on all configured repositories asynchronously.
        /// </summary>
        /// <returns> Whether any repository failed to persist changes or not. </returns>
        public virtual async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            InternalConfigure();

            var result = true;
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var repository in Repositories)
                result &= await repository.SaveChangesAsync(cancellationToken);
            return result;
        }

        /// <summary>
        /// Override to perform any custom configuration on the unit of work such as adding repositories.
        /// Will be called before <see cref="Repository(Type)"/>, <see cref="Repository{TEntity}"/>,
        /// <see cref="SaveChanges"/>, and <see cref="SaveChangesAsync"/>.
        /// Will only ever be called once.
        /// </summary>
        // ReSharper disable once VirtualMemberNeverOverridden.Global
        protected virtual void Configure() { }

        private void InternalConfigure()
        {
            if (_configured) return;

            Configure();

            _configured = true;
        }

        #region IDisposable Support

        private bool _disposedValue; // To detect redundant calls

        /// <summary>
        ///     Disposes any repositories that implement IDisposable.
        /// </summary>
        /// <param name="disposing"> Whether the object is currently being disposed </param>
        // ReSharper disable once VirtualMemberNeverOverridden.Global
        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;

            if (disposing)
            {
                foreach (var repository in Repositories)
                {
                    if (repository is IDisposable disposable)
                        disposable.Dispose();
                }
            }

            _disposedValue = true;
        }

        /// <summary>
        ///     Disposes any repositories that implement IDisposable.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
