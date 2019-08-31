using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace UnMango.Extensions.Repository.EntityFramework
{
    /// <summary>
    ///     The repository is an implementation of the repository pattern for <typeparamref name="TEntity"/>
    /// </summary>
    /// <typeparam name="TEntity">Type of entity the repository manages</typeparam>
    public abstract class Repository<TEntity> : DbSet<TEntity>, IRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        ///     Initializes a new instance of the repository using the <paramref name="context"/>
        ///     as the underlying database connection
        /// </summary>
        /// <param name="context"></param>
        // ReSharper disable once PublicConstructorInAbstractClass
        public Repository([NotNull] DbContext context) => Context = Check.NotNull(context, nameof(context));

        /// <summary>
        ///     The underlying Entity Framework Core <see cref="DbContext"/>
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        protected internal DbContext Context { get; }

        /// <summary>
        ///     Use <see cref="IRepository{TEntity}.Add(TEntity)"/> and call <see cref="IRepository{TEntity}.SaveChangesAsync"/>
        /// </summary>
        /// <param name="entity"> The entity to add. </param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        [Obsolete("Entity Framework does not support AddAsync")]
        public virtual Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
            => throw new NotSupportedException($"{nameof(AddAsync)}");

        /// <summary>
        ///     Use <see cref="IRepository{TEntity}.AddRange(IEnumerable{TEntity})"/> and call <see cref="IRepository{TEntity}.SaveChangesAsync"/>
        /// </summary>
        /// <param name="entities"> The entities to add. </param>
        /// <returns> A task that represents the asynchronous operation. </returns>
        [Obsolete("Entity Framework does not support AddRangeAsync")]
        public virtual Task AddRangeAsync(params TEntity[] entities)
            => throw new NotSupportedException($"Entity Framework does not support {nameof(AddRangeAsync)}");

        /// <summary>
        ///     Use <see cref="IRepository{TEntity}.AddRange(IEnumerable{TEntity})"/> and call <see cref="IRepository{TEntity}.SaveChangesAsync"/>
        /// </summary>
        /// <param name="entities"> The entities to add. </param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns> A task that represents the asynchronous operation. </returns>
        [Obsolete("Entity Framework does not support AddRangeAsync")]
        public virtual Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
            => throw new NotSupportedException($"Entity Framework does not support {nameof(AddRangeAsync)}");

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entities in the <see cref="EntityState.Unchanged" /> state
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
        /// <param name="entities"> The entities to attach. </param>
        public virtual void AttachRange(params TEntity[] entities)
        {
            foreach (var entity in entities)
                Attach(entity);
        }

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entities in the <see cref="EntityState.Unchanged" /> state
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
        /// <param name="entities"> The entities to attach. </param>
        public virtual void AttachRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
                Attach(entity);
        }

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entity in the <see cref="EntityState.Modified" /> state such that it will
        ///         be updated in the database when <see cref="DbContext.SaveChanges()" /> is called.
        ///     </para>
        ///     <para>
        ///         All properties of the entity will be marked as modified.
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
        /// <param name="entity"> The entity to update. </param>
        public virtual void Update(TEntity entity)
        {
            var props = Check.NotNull(entity, nameof(entity)).GetType().GetProperties();
            var keyProp = props.FirstOrDefault(x => x.GetCustomAttributes(typeof(KeyAttribute), false).Any());
            if (keyProp == null)
                keyProp = props.FirstOrDefault(x => x.Name.Contains("Id", StringComparison.OrdinalIgnoreCase));
            if (keyProp == null)
                keyProp = props.FirstOrDefault();

            var toUpdate = Find(keyProp?.GetValue(entity));
            Context.Entry(toUpdate).CurrentValues.SetValues(entity);
        }

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entities in the <see cref="EntityState.Modified" /> state such that they will
        ///         be updated in the database when <see cref="DbContext.SaveChanges()" /> is called.
        ///     </para>
        ///     <para>
        ///         All properties of each entity will be marked as modified.
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
        /// <param name="entities"> The entities to update. </param>
        public virtual void UpdateRange(params TEntity[] entities)
        {
            foreach (var entity in entities)
                Update(entity);
        }

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entities in the <see cref="EntityState.Modified" /> state such that they will
        ///         be updated in the database when <see cref="DbContext.SaveChanges()" /> is called.
        ///     </para>
        ///     <para>
        ///         All properties of each entity will be marked as modified.
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
        /// <param name="entities"> The entities to update. </param>
        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
                Update(entity);
        }

        #region IRepository Implementation

        void IRepository<TEntity>.Add(TEntity entity) => base.Add(entity);

        void IRepository<TEntity>.AddRange(params TEntity[] entities) => base.AddRange(entities);

        void IRepository<TEntity>.AddRange(IEnumerable<TEntity> entities) => base.AddRange(entities);

        Task<TEntity> IRepository<TEntity>.FindAsync(object[] keyValues, CancellationToken cancellationToken)
            => base.FindAsync(keyValues, cancellationToken);

        void IRepository<TEntity>.Remove(TEntity entity) => base.Remove(entity);

        void IRepository<TEntity>.RemoveRange(params TEntity[] entities) => base.RemoveRange(entities);

        void IRepository<TEntity>.RemoveRange(IEnumerable<TEntity> entities) => base.RemoveRange(entities);

        bool IRepository.SaveChanges() => Context == null || Context.SaveChanges() >= 0;

        async Task<bool> IRepository.SaveChangesAsync(CancellationToken cancellationToken)
            => Context == null || await Context.SaveChangesAsync(cancellationToken) >= 0;

        #endregion
    }
}
