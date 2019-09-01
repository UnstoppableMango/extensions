using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace UnMango.Extensions.Repository.EntityFrameworkCore
{
    /// <summary>
    /// Extension methods for <see cref="IUnitOfWork"/>.
    /// </summary>
    public static class UnitOfWorkExtensions
    {
        private const string INVALID_UOW_MESSAGE = "Underlying unit of work is not a " +
            "KG.Data.EntityFrameworkCore.UnitOfWork";

        /// <summary>
        /// Casts the output of <see cref="DbContext.Set{TEntity}"/> as a <see cref="Repository{TEntity}"/>.
        /// Gets the <see cref="IRepository{TEntity}"/> for the specified <typeparamref name="TEntity"/>.
        /// Will throw an exception if <paramref name="unitOfWork"/> does not implement <see cref="UnitOfWork"/>.
        /// </summary>
        /// <typeparam name="TEntity">The <see cref="IRepository{TEntity}"/> to retrieve.</typeparam>
        /// <param name="unitOfWork">The <see cref="IUnitOfWork"/> scope to use.</param>
        /// <returns>The <see cref="IRepository{TEntity}"/> for the specifed entity type.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when <paramref name="unitOfWork"/> does not implement <see cref="UnitOfWork"/>.
        /// </exception>
        [UsedImplicitly]
        public static Repository<TEntity> Repository<TEntity>(this IUnitOfWork unitOfWork)
            where TEntity : class
            => (Repository<TEntity>)Check(unitOfWork).Set<TEntity>();

        /// <summary>
        ///     Saves all changes made in this context to the database.
        /// </summary>
        /// <param name="repository"> The repository to operate on. </param>
        /// <param name="acceptAllChangesOnSuccess">
        ///     Indicates whether <see cref="ChangeTracker.AcceptAllChanges" /> is called after the changes have
        ///     been sent successfully to the database.
        /// </param>
        /// <remarks>
        ///     This method will automatically call <see cref="ChangeTracker.DetectChanges" /> to discover any
        ///     changes to entity instances before saving to the underlying database. This can be disabled via
        ///     <see cref="ChangeTracker.AutoDetectChangesEnabled" />.
        /// </remarks>
        /// <returns>
        ///     Whether the operation was successful or not.
        /// </returns>
        /// <exception cref="DbUpdateException">
        ///     An error is encountered while saving to the database.
        /// </exception>
        /// <exception cref="DbUpdateConcurrencyException">
        ///     A concurrency violation is encountered while saving to the database.
        ///     A concurrency violation occurs when an unexpected number of rows are affected during save.
        ///     This is usually because the data in the database has been modified since it was loaded into memory.
        /// </exception>
        [UsedImplicitly]
        public static int SaveChanges(this IUnitOfWork uow, bool acceptAllChangesOnSuccess = true)
            => Check(uow).SaveChanges(acceptAllChangesOnSuccess);

        /// <summary>
        ///     Asynchronously saves all changes made in this context to the database.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         This method will automatically call <see cref="ChangeTracker.DetectChanges" /> to discover any
        ///         changes to entity instances before saving to the underlying database. This can be disabled via
        ///         <see cref="ChangeTracker.AutoDetectChangesEnabled" />.
        ///     </para>
        ///     <para>
        ///         Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
        ///         that any asynchronous operations have completed before calling another method on this context.
        ///     </para>
        /// </remarks>
        /// <param name="repository"> The repository to operate on. </param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        ///     A task that represents the asynchronous save operation.
        /// </returns>
        /// <exception cref="DbUpdateException">
        ///     An error is encountered while saving to the database.
        /// </exception>
        /// <exception cref="DbUpdateConcurrencyException">
        ///     A concurrency violation is encountered while saving to the database.
        ///     A concurrency violation occurs when an unexpected number of rows are affected during save.
        ///     This is usually because the data in the database has been modified since it was loaded into memory.
        /// </exception>
        [UsedImplicitly]
        public static Task<int> SaveChangesAsync(this IUnitOfWork uow, CancellationToken cancellationToken = default)
            => Check(uow).SaveChangesAsync(cancellationToken);

        /// <summary>
        ///     Asynchronously saves all changes made in this context to the database.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">
        ///     Indicates whether <see cref="ChangeTracker.AcceptAllChanges" /> is called after the changes have
        ///     been sent successfully to the database.
        /// </param>
        /// <remarks>
        ///     <para>
        ///         This method will automatically call <see cref="ChangeTracker.DetectChanges" /> to discover any
        ///         changes to entity instances before saving to the underlying database. This can be disabled via
        ///         <see cref="ChangeTracker.AutoDetectChangesEnabled" />.
        ///     </para>
        ///     <para>
        ///         Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
        ///         that any asynchronous operations have completed before calling another method on this context.
        ///     </para>
        /// </remarks>
        /// <param name="repository"> The repository to operate on. </param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        ///     A task that represents the asynchronous save operation.
        /// </returns>
        /// <exception cref="DbUpdateException">
        ///     An error is encountered while saving to the database.
        /// </exception>
        /// <exception cref="DbUpdateConcurrencyException">
        ///     A concurrency violation is encountered while saving to the database.
        ///     A concurrency violation occurs when an unexpected number of rows are affected during save.
        ///     This is usually because the data in the database has been modified since it was loaded into memory.
        /// </exception>
        [UsedImplicitly]
        public static Task<int> SaveChangesAsync(
            this IUnitOfWork uow,
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
            => Check(uow).SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        private static UnitOfWork Check(IUnitOfWork unitOfWork)
        {
            if (!(unitOfWork is UnitOfWork unitOfWorkBase))
                throw new InvalidOperationException(INVALID_UOW_MESSAGE);
            return unitOfWorkBase;
        }
    }
}
