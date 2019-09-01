using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using UnMango.Extensions.Repository.EntityFrameworkCore.Internal;

namespace UnMango.Extensions.Repository.EntityFrameworkCore
{
    /// <summary>
    ///     Extension methods for common Entity Framework Core operations
    /// </summary>
    public static class RepositoryExtensions
    {
        private const string INVALID_REPO_MESSAGE = "Underlying repository is not an Entity Framework Core repo";

        /// <summary>
        ///     Begins tracking the given entity, and any other reachable entities that are
        ///     not already being tracked, in the <see cref="EntityState.Added" /> state such that they will
        ///     be inserted into the database when <see cref="DbContext.SaveChanges()" /> is called.
        /// </summary>
        /// <param name="repository">The <see cref="IRepository{TEntity}"/> to perform the operation on.</param>
        /// <param name="entity"> The entity to add. </param>
        /// <returns>
        ///     The <see cref="EntityEntry{TEntity}" /> for the entity. The entry provides
        ///     access to change tracking information and operations for the entity.
        /// </returns>
        [UsedImplicitly]
        public static EntityEntry<TEntity> Add<TEntity>(
            [NotNull] this IRepository<TEntity> repository,
            [NotNull] TEntity entity)
            where TEntity : class
            => Check(repository).Add(entity);

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entity, and any other reachable entities that are
        ///         not already being tracked, in the <see cref="EntityState.Added" /> state such that they will
        ///         be inserted into the database when <see cref="DbContext.SaveChanges()" /> is called.
        ///     </para>
        ///     <para>
        ///         This method is async only to allow special value generators, such as the one used by
        ///         'Microsoft.EntityFrameworkCore.Metadata.SqlServerValueGenerationStrategy.SequenceHiLo',
        ///         to access the database asynchronously. For all other cases the non async method should be used.
        ///     </para>
        /// </summary>
        /// <param name="repository">The <see cref="IRepository{TEntity}"/> to perform the operation on.</param>
        /// <param name="entity"> The entity to add. </param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        ///     A task that represents the asynchronous Add operation. The task result contains the
        ///     <see cref="EntityEntry{TEntity}" /> for the entity. The entry provides access to change tracking
        ///     information and operations for the entity.
        /// </returns>
        [UsedImplicitly]
        public static Task<EntityEntry<TEntity>> AddAsync<TEntity>(
            [NotNull] this IRepository<TEntity> repository,
            [NotNull] TEntity entity,
            CancellationToken cancellationToken = default)
            where TEntity : class
            => Check(repository).AddAsync(entity, cancellationToken);

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entity in the <see cref="EntityState.Unchanged" /> state
        ///         such that no operation will be performed when <see cref="DbContext.SaveChanges()" />
        ///         is called.
        ///     </para>
        ///     <para>
        ///         A recursive search of the navigation properties will be performed to find reachable entities
        ///         that are not already being tracked by the context. These entities will also begin to be tracked
        ///         by the context. If a reachable entity has its primary key value set
        ///         then it will be tracked in the <see cref="EntityState.Unchanged" /> state. If the primary key
        ///         value is not set then it will be tracked in the <see cref="EntityState.Added" /> state.
        ///         An entity is considered to have its primary key value set if the primary key property is set
        ///         to anything other than the CLR default for the property type.
        ///     </para>
        /// </summary>
        /// <param name="repository">The <see cref="IRepository{TEntity}"/> to perform the operation on.</param>
        /// <param name="entity"> The entity to attach. </param>
        /// <returns>
        ///     The <see cref="EntityEntry" /> for the entity. The entry provides
        ///     access to change tracking information and operations for the entity.
        /// </returns>
        [UsedImplicitly]
        public static EntityEntry<TEntity> Attach<TEntity>(
            [NotNull] this IRepository<TEntity> repository,
            [NotNull] TEntity entity)
            where TEntity : class
            => Check(repository).Attach(entity);

        /// <summary>
        ///     Begins tracking the given entity in the <see cref="EntityState.Deleted" /> state such that it will
        ///     be removed from the database when <see cref="DbContext.SaveChanges()" /> is called.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         If the entity is already tracked in the <see cref="EntityState.Added" /> state then the context will
        ///         stop tracking the entity (rather than marking it as <see cref="EntityState.Deleted" />) since the
        ///         entity was previously added to the context and does not exist in the database.
        ///     </para>
        ///     <para>
        ///         Any other reachable entities that are not already being tracked will be tracked in the same way that
        ///         they would be if <see cref="DbSet{TEntity}.Attach(TEntity)" /> was called before calling this method.
        ///         This allows any cascading actions to be applied when <see cref="DbContext.SaveChanges()" /> is called.
        ///     </para>
        /// </remarks>
        /// <param name="repository">The <see cref="IRepository{TEntity}"/> to perform the operation on.</param>
        /// <param name="entity"> The entity to remove. </param>
        /// <returns>
        ///     The <see cref="EntityEntry{TEntity}" /> for the entity. The entry provides
        ///     access to change tracking information and operations for the entity.
        /// </returns>
        [UsedImplicitly]
        public static EntityEntry<TEntity> Remove<TEntity>(
            [NotNull] this IRepository<TEntity> repository,
            [NotNull] TEntity entity)
            where TEntity : class
            => Check(repository).Remove(entity);

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entity in the <see cref="EntityState.Modified" /> state such that it will
        ///         be updated in the database when <see cref="DbContext.SaveChanges()" /> is called.
        ///     </para>
        ///     <para>
        ///         All properties of the entity will be marked as modified. To mark only some properties as modified, use
        ///         <see cref="DbSet{TEntity}.Attach(TEntity)" /> to begin tracking the entity in the <see cref="EntityState.Unchanged" />
        ///         state and then use the returned <see cref="EntityEntry" /> to mark the desired properties as modified.
        ///     </para>
        ///     <para>
        ///         A recursive search of the navigation properties will be performed to find reachable entities
        ///         that are not already being tracked by the context. These entities will also begin to be tracked
        ///         by the context. If a reachable entity has its primary key value set
        ///         then it will be tracked in the <see cref="EntityState.Modified" /> state. If the primary key
        ///         value is not set then it will be tracked in the <see cref="EntityState.Added" /> state.
        ///         An entity is considered to have its primary key value set if the primary key property is set
        ///         to anything other than the CLR default for the property type.
        ///     </para>
        /// </summary>
        /// <param name="repository">The <see cref="IRepository{TEntity}"/> to perform the operation on.</param>
        /// <param name="entity"> The entity to update. </param>
        /// <returns>
        ///     The <see cref="EntityEntry" /> for the entity. The entry provides
        ///     access to change tracking information and operations for the entity.
        /// </returns>
        [UsedImplicitly]
        public static EntityEntry<TEntity> Update<TEntity>(
            [NotNull] this IRepository<TEntity> repository,
            [NotNull] TEntity entity)
            where TEntity : class
            => Check(repository).Update(entity);

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
        public static int SaveChanges<TEntity>(
            [NotNull] this IRepository<TEntity> repository,
            bool acceptAllChangesOnSuccess = true)
            where TEntity : class
            => Check(repository).Context.SaveChanges(acceptAllChangesOnSuccess);

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
        public static Task<int> SaveChangesAsync<TEntity>(
            this IRepository<TEntity> repository,
            CancellationToken cancellationToken = default)
            where TEntity : class
            => Check(repository).Context.SaveChangesAsync(cancellationToken);

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
        public static async Task<int> SaveChangesAsync<TEntity>(
            this IRepository<TEntity> repository,
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
            where TEntity : class
            => await Check(repository).Context.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        // ReSharper disable once ParameterTypeCanBeEnumerable.Local
        private static IInternalRepository<TEntity> Check<TEntity>(IRepository<TEntity> repository)
            where TEntity : class
        {
            if (!(repository is IInternalRepository<TEntity> efCoreRepository))
                throw new InvalidOperationException(INVALID_REPO_MESSAGE);
            return efCoreRepository;
        }
    }
}
