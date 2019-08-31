using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace KG.Data.MongoDB
{
    /// <summary>
    ///     The repository is an implementation of the repository pattern for <typeparamref name="TEntity"/>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly IMongoDatabase _mongoDatabase;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Repository{TEntity}"/> class with the specified MongoDB
        /// </summary>
        /// <param name="mongoDatabase"></param>
        protected Repository(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
        }

        /// <summary>
        ///     The collection of <typeparamref name="TEntity"/>s this repository is responsible for
        /// </summary>
        protected internal IMongoCollection<TEntity> Entities
            => _mongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name);

        /// <summary>
        ///     Adds a single <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="entity"> The entity to be added. </param>
        public virtual void Add(TEntity entity) => Entities.InsertOne(entity);

        /// <summary>
        ///     Adds a single <typeparamref name="TEntity"/> asynchronously.
        /// </summary>
        /// <param name="entity"> The entity to be added. </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken" /> to observe while waiting for the task to complete. </param>
        /// <returns> A task representing the add operation. </returns>
        public virtual Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
            => Entities.InsertOneAsync(entity, null, cancellationToken);

        /// <summary>
        ///     Adds all <typeparamref name="TEntity"/>s.
        /// </summary>
        /// <param name="entities"> The entities to be added. </param>
        public virtual void AddRange(params TEntity[] entities) => Entities.InsertMany(entities);

        /// <summary>
        ///     Adds all <typeparamref name="TEntity"/>s.
        /// </summary>
        /// <param name="entities"> The entities to be added. </param>
        public virtual void AddRange(IEnumerable<TEntity> entities) => Entities.InsertMany(entities);

        /// <summary>
        ///     Adds all <typeparamref name="TEntity"/>s asynchronously.
        /// </summary>
        /// <param name="entities"> The entities to be added. </param>
        /// <returns> A task representing the add operation. </returns>
        public virtual Task AddRangeAsync(params TEntity[] entities) => Entities.InsertManyAsync(entities);

        /// <summary>
        ///     Adds all <typeparamref name="TEntity"/>s asynchronously.
        /// </summary>
        /// <param name="entities"> The entities to be added. </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken" /> to observe while waiting for the task to complete. </param>
        /// <returns> A task representing the add operation. </returns>
        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
            => await Entities.InsertManyAsync(entities, null, cancellationToken);

        [Obsolete("MongoDB does not support tracking")]
        public virtual void Attach(TEntity entity) => throw new NotSupportedException("MongoDB does not support tracking");

        [Obsolete("MongoDB does not support tracking")]
        public virtual void AttachRange(params TEntity[] entities) => throw new NotSupportedException("MongoDB does not support tracking");

        [Obsolete("MongoDB does not support tracking")]
        public virtual void AttachRange(IEnumerable<TEntity> entities) => throw new NotSupportedException("MongoDB does not support tracking");

        /// <summary>
        ///     Finds a single <typeparamref name="TEntity"/> with the specified key values.
        /// </summary>
        /// <param name="keyValues"> Key values to search for. </param>
        /// <returns> The <typeparamref name="TEntity"/> matching the specified key values. </returns>
        public virtual TEntity Find(params object[] keyValues)
        {
            var filter = Helpers.GetAndKeyFilter<TEntity>(keyValues);

            return Entities.Find(filter).Single();
        }

        /// <summary>
        ///     Finds a single <typeparamref name="TEntity"/> with the specified key values.
        /// </summary>
        /// <param name="keyValues"> Key values to search for. </param>
        /// <returns> A task returning the <typeparamref name="TEntity"/> matching the specified key values. </returns>
        public virtual Task<TEntity> FindAsync(params object[] keyValues) => FindAsync(keyValues, default);

        /// <summary>
        ///     Finds a single <typeparamref name="TEntity"/> with the specified key values.
        /// </summary>
        /// <param name="keyValues"> Key values to search for. </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken" /> to observe while waiting for the task to complete. </param>
        /// <returns> A task returning the <typeparamref name="TEntity"/> matching the specified key values. </returns>
        public virtual async Task<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken)
        {
            var filter = Helpers.GetAndKeyFilter<TEntity>(keyValues);

            var findResult = await Entities.FindAsync(filter, null, cancellationToken);
            return await findResult.SingleAsync(cancellationToken);
        }

        /// <summary>
        ///     Removes a single <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="entity"> <typeparamref name="TEntity"/> to be removed. </param>
        public virtual void Remove(TEntity entity)
        {
            var result = Entities.DeleteOne(x => x == entity);
            if (!result.IsAcknowledged)
                throw new Exception("The remove operation was not persisted");
        }

        /// <summary>
        ///     Removes all <typeparamref name="TEntity"/>s.
        /// </summary>
        /// <param name="entities"> <typeparamref name="TEntity"/>s to be removed. </param>
        public virtual void RemoveRange(params TEntity[] entities)
        {
            var result = Entities.DeleteMany(x => entities.Contains(x));
            if (!result.IsAcknowledged)
                throw new Exception("The remove operation was not persisted");
        }

        /// <summary>
        ///     Removes all <typeparamref name="TEntity"/>s.
        /// </summary>
        /// <param name="entities"> <typeparamref name="TEntity"/>s to be removed. </param>
        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            var result = Entities.DeleteMany(x => entities.Contains(x));
            if (!result.IsAcknowledged)
                throw new Exception("The remove operation was not persisted");
        }

        [Obsolete("Use the Update<TEntity>() extension methods instead.")]
        public virtual void Update(TEntity entity)
            => throw new NotSupportedException("Use one of the extension methods");

        [Obsolete("Use the UpdateRange<TEntity>() extension methods instead.")]
        public virtual void UpdateRange(params TEntity[] entities)
            => throw new NotSupportedException("Use one of the extension methods");
        
        [Obsolete("Use the UpdateRange<TEntity>() extension methods instead.")]
        public virtual void UpdateRange(IEnumerable<TEntity> entities)
            => throw new NotSupportedException("Use one of the extension methods");

        [Obsolete("Mongo persists changes as they happen")]
        public virtual bool SaveChanges() => true;

        [Obsolete("Mongo persists changes as they happen")]
        public virtual Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
            => Task.FromResult(true);

        #region Queryable and Enumerable Support

        Type IQueryable.ElementType => Entities.AsQueryable().ElementType;
        Expression IQueryable.Expression => Entities.AsQueryable().Expression;
        IQueryProvider IQueryable.Provider => Entities.AsQueryable().Provider;
        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator() => Entities.AsQueryable().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Entities.AsQueryable().GetEnumerator();
        
        #endregion

        public IList GetList() => throw new NotImplementedException();

        public bool ContainsListCollection { get; }
    }
}
