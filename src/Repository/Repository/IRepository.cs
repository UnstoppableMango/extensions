using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KG.Data
{
    /// <inheritdoc />
    /// <summary>
    ///     An implementation of the repository pattern
    /// </summary>
    public interface IRepository : IEnumerable
    {
        /// <summary>
        ///     Saves all changes made in this repository to the underlying data store.
        /// </summary>
        /// <returns> Whether the operation was successful or not. </returns>
        bool SaveChanges();

        /// <summary>
        ///     Asynchronously saves all changes made in this repository to the underlying data store.
        /// </summary>
        /// <remarks>
        ///     Multiple active operations on the same repository instance are not supported.  Use 'await' to ensure
        ///     that any asynchronous operations have completed before calling another method on this repository.
        /// </remarks>
        /// <returns> A task that represents the asynchronous save operation. </returns>
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    /// <summary>
    ///     An implementation of the repository pattern for <typeparamref name="TEntity"/>
    /// </summary>
    /// <typeparam name="TEntity">Type of entity the repository manages</typeparam>
    // ReSharper disable once InheritdocConsiderUsage
    public interface IRepository<TEntity> : IQueryable<TEntity>, IListSource, IRepository
        where TEntity : class
    {
        /// <summary>
        ///     Finds an entity with the given primary key values. If an entity with the given primary key values
        ///     is being tracked, then it is returned immediately without making a request to the
        ///     underlying data store. Otherwise, request is made to the store for an entity with the given primary key values
        ///     and this entity, if found, is attached and returned. If no entity is found, then
        ///     null is returned.
        /// </summary>
        /// <param name="keyValues"> The values of the primary key for the entity to be found. </param>
        /// <returns> The entity found, or null. </returns>
        [UsedImplicitly]
        TEntity Find([CanBeNull] params object[] keyValues);

        /// <summary>
        ///     Finds an entity with the given primary key values. If an entity with the given primary key values
        ///     is being tracked, then it is returned immediately without making a request to the
        ///     underlying data store. Otherwise, request is made to the store for an entity with the given primary key values
        ///     and this entity, if found, is attached and returned. If no entity is found, then
        ///     null is returned.
        /// </summary>
        /// <param name="keyValues"> The values of the primary key for the entity to be found. </param>
        /// <returns> A task representing either the entity found, or null. </returns>
        [UsedImplicitly]
        Task<TEntity> FindAsync([CanBeNull] params object[] keyValues);

        /// <summary>
        ///     Finds an entity with the given primary key values. If an entity with the given primary key values
        ///     is being tracked, then it is returned immediately without making a request to the
        ///     underlying data store. Otherwise, request is made to the store for an entity with the given primary key values
        ///     and this entity, if found, is attached and returned. If no entity is found, then
        ///     null is returned.
        /// </summary>
        /// <param name="keyValues"> The values of the primary key for the entity to be found. </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken" /> to observe while waiting for the task to complete. </param>
        /// <returns> A task representing either the entity found, or null. </returns>
        [UsedImplicitly]
        Task<TEntity> FindAsync([CanBeNull] object[] keyValues, CancellationToken cancellationToken);

        /// <summary>
        ///     Begins tracking the given entity, and any other reachable entities that are
        ///     not already being tracked, such that they will be persisted 
        ///     when <see cref="IRepository.SaveChanges()" /> is called.
        /// </summary>
        /// <param name="entity"> The entity to add. </param>
        void Add([NotNull] TEntity entity);

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entity, and any other reachable entities that are
        ///         not already being tracked, such that they will be persisted 
        ///         when <see cref="IRepository.SaveChanges()" /> is called.
        ///     </para>
        ///     <para>
        ///         This method is async only to allow special value generators, such as the one used by
        ///         'Microsoft.EntityFrameworkCore.Metadata.SqlServerValueGenerationStrategy.SequenceHiLo',
        ///         to access the repository asynchronously. For all other cases the non async method should be used.
        ///     </para>
        /// </summary>
        /// <param name="entity"> The entity to add. </param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        [UsedImplicitly]
        Task AddAsync([NotNull] TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Begins tracking the given entity in the "Deleted" state such that it will be removed
        ///     from the underlying data store when <see cref="IRepository.SaveChanges()" /> is called.
        /// </summary>
        /// <param name="entity"> The entity to remove. </param>
        [UsedImplicitly]
        void Remove([NotNull] TEntity entity);

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entity in the "Modified" state such that it will be updated
        ///         in the underlying data store when <see cref="IRepository.SaveChanges()" /> is called.
        ///     </para>
        ///     <para>
        ///         All properties of the entity will be marked as modified.
        ///     </para>
        /// </summary>
        /// <param name="entity"> The entity to update. </param>
        [UsedImplicitly]
        void Update([NotNull] TEntity entity);

        /// <summary>
        ///     Begins tracking the given entities, and any other reachable entities that are
        ///     not already being tracked, such that they will be persisted 
        ///     when <see cref="IRepository.SaveChanges()" /> is called.
        /// </summary>
        /// <param name="entities"> The entities to add. </param>
        [UsedImplicitly]
        void AddRange([NotNull] params TEntity[] entities);

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entities, and any other reachable entities that are
        ///         not already being tracked, such that they will be persisted 
        ///         when <see cref="IRepository.SaveChanges()" /> is called.
        ///     </para>
        ///     <para>
        ///         This method is async only to allow special value generators, such as the one used by
        ///         'Microsoft.EntityFrameworkCore.Metadata.SqlServerValueGenerationStrategy.SequenceHiLo',
        ///         to access the database asynchronously. For all other cases the non async method should be used.
        ///     </para>
        /// </summary>
        /// <param name="entities"> The entities to add. </param>
        /// <returns> A task that represents the asynchronous operation. </returns>
        [UsedImplicitly]
        Task AddRangeAsync([NotNull] params TEntity[] entities);

        /// <summary>
        ///     Begins tracking the given entities in the "Unchanged" state
        ///     such that no operation will be performed when <see cref="IRepository.SaveChanges()" />
        ///     is called.
        /// </summary>
        /// <param name="entities"> The entities to attach. </param>
        [UsedImplicitly]
        void AttachRange([NotNull] params TEntity[] entities);

        /// <summary>
        ///     Begins tracking the given entities in the "Unchanged" state
        ///     such that no operation will be performed when <see cref="IRepository.SaveChanges()" />
        ///     is called.
        /// </summary>
        /// <param name="entities"> The entities to attach. </param>
        [UsedImplicitly]
        void AttachRange([NotNull] IEnumerable<TEntity> entities);

        /// <summary>
        ///     Begins tracking the given entities in the "Deleted" state such that they will be removed
        ///     from the underlying data store when <see cref="IRepository.SaveChanges()" /> is called.
        /// </summary>
        /// <param name="entities"> The entities to remove. </param>
        [UsedImplicitly]
        void RemoveRange([NotNull] params TEntity[] entities);

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entity in the "Modified" state such that it will be updated
        ///         in the underlying data store when <see cref="IRepository.SaveChanges()" /> is called.
        ///     </para>
        ///     <para>
        ///         All properties of each entity will be marked as modified.
        ///     </para>
        /// </summary>
        /// <param name="entities"> The entities to update. </param>
        [UsedImplicitly]
        void UpdateRange([NotNull] params TEntity[] entities);

        /// <summary>
        ///     Begins tracking the given entities, and any other reachable entities that are
        ///     not already being tracked, such that they will be persisted 
        ///     when <see cref="IRepository.SaveChanges()" /> is called.
        /// </summary>
        /// <param name="entities"> The entities to add. </param>
        [UsedImplicitly]
        void AddRange([NotNull] IEnumerable<TEntity> entities);

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entity, and any other reachable entities that are
        ///         not already being tracked, such that they will be persisted 
        ///         when <see cref="IRepository.SaveChanges()" /> is called.
        ///     </para>
        ///     <para>
        ///         This method is async only to allow special value generators, such as the one used by
        ///         'Microsoft.EntityFrameworkCore.Metadata.SqlServerValueGenerationStrategy.SequenceHiLo',
        ///         to access the database asynchronously. For all other cases the non async method should be used.
        ///     </para>
        /// </summary>
        /// <param name="entities"> The entities to add. </param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns> A task that represents the asynchronous operation. </returns>
        [UsedImplicitly]
        Task AddRangeAsync([NotNull] IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Begins tracking the given entities in the "Deleted" state such that they will be removed
        ///     from the underlying data store when <see cref="IRepository.SaveChanges()" /> is called.
        /// </summary>
        /// <param name="entities"> The entities to remove. </param>
        [UsedImplicitly]
        void RemoveRange([NotNull] IEnumerable<TEntity> entities);

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entity in the "Modified" state such that it will be updated
        ///         in the underlying data store when <see cref="IRepository.SaveChanges()" /> is called.
        ///     </para>
        ///     <para>
        ///         All properties of each entity will be marked as modified.
        ///     </para>
        /// </summary>
        /// <param name="entities"> The entities to update. </param>
        [UsedImplicitly]
        void UpdateRange([NotNull] IEnumerable<TEntity> entities);
    }
}
