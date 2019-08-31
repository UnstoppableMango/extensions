using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace KG.Data.MongoDB
{
    /// <summary>
    ///     Methods to fully take advantage of the features MongoDB has to offer
    /// </summary>
    public static class MongoIRepositoryExtensions
    {
        private const string INVALID_REPO_MESSAGE = "Underlying repository is not a mongoDb repo";

        /// <summary>
        ///     Adds a single <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity"> Type of the entity to be added. </typeparam>
        /// <param name="repository"> <see cref="Repository{TEntity}"/> to add the entity to. </param>
        /// <param name="entity"> The <typeparamref name="TEntity"/> to be added. </param>
        /// <param name="options"> MongoDB insert options. </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken" /> to observe while waiting for the task to complete. </param>
        public static void Add<TEntity>(this IRepository<TEntity> repository, TEntity entity, InsertOneOptions options = null, CancellationToken cancellationToken = default)
            where TEntity : class
            => Check(repository).Entities.InsertOne(entity, options, cancellationToken);

        /// <summary>
        ///     Adds a single <typeparamref name="TEntity"/> asynchronously.
        /// </summary>
        /// <typeparam name="TEntity"> Type of the entity to be added. </typeparam>
        /// <param name="repository"> <see cref="Repository{TEntity}"/> to add the entity to. </param>
        /// <param name="entity"> The <typeparamref name="TEntity"/> to be added. </param>
        /// <param name="options"> MongoDB insert options. </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken" /> to observe while waiting for the task to complete. </param>
        /// <returns> A <see cref="Task"/> representing the add operation. </returns>
        public static Task AddAsync<TEntity>(this IRepository<TEntity> repository, TEntity entity, InsertOneOptions options = null, CancellationToken cancellationToken = default)
            where TEntity : class
            => Check(repository).Entities.InsertOneAsync(entity, options, cancellationToken);

        /// <summary>
        ///     Adds all <typeparamref name="TEntity"/>s.
        /// </summary>
        /// <typeparam name="TEntity"> Type of the entity to be added. </typeparam>
        /// <param name="repository"> <see cref="Repository{TEntity}"/> to add the entity to. </param>
        /// <param name="entities"> The <typeparamref name="TEntity"/>s to be added. </param>
        /// <param name="options"> MongoDB insert options. </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken" /> to observe while waiting for the task to complete. </param>
        public static void AddRange<TEntity>(this IRepository<TEntity> repository, IEnumerable<TEntity> entities, InsertManyOptions options = null, CancellationToken cancellationToken = default)
            where TEntity : class
            => Check(repository).Entities.InsertMany(entities, options, cancellationToken);

        /// <summary>
        ///     Adds all <typeparamref name="TEntity"/>s asynchronously.
        /// </summary>
        /// <typeparam name="TEntity"> Type of the entity to be added. </typeparam>
        /// <param name="repository"> <see cref="Repository{TEntity}"/> to add the entity to. </param>
        /// <param name="entities"> The <typeparamref name="TEntity"/>s to be added. </param>
        /// <param name="options"> MongoDB insert options. </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken" /> to observe while waiting for the task to complete. </param>
        /// <returns> A <see cref="Task"/> representing the add operation. </returns>
        public static Task AddRangeAsync<TEntity>(this IRepository<TEntity> repository, IEnumerable<TEntity> entities, InsertManyOptions options = null, CancellationToken cancellationToken = default)
            where TEntity : class
            => Check(repository).Entities.InsertManyAsync(entities, options, cancellationToken);

        /// <summary>
        ///     Watches changes on the <see cref="IMongoCollection{TDocument}"/>.
        /// </summary>
        /// <typeparam name="TEntity"> Type of the entity to be added. </typeparam>
        /// <param name="repository"> <see cref="Repository{TEntity}"/> to add the entity to. </param>
        /// <param name="options"> MongoDB options for a change stream operation. </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken" /> to observe while waiting for the task to complete. </param>
        /// <returns> An asynchronous cursor referencing an output document from a $changestream pipeline stage. </returns>
        public static IAsyncCursor<ChangeStreamDocument<TEntity>> Watch<TEntity>(this IRepository<TEntity> repository, ChangeStreamOptions options = null, CancellationToken cancellationToken = default)
            where TEntity : class
            => Check(repository).Entities.Watch(options, cancellationToken);

        /// <summary>
        ///     Finds a single <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity"> Type of the entity to be added. </typeparam>
        /// <param name="repository"> <see cref="Repository{TEntity}"/> to add the entity to. </param>
        /// <param name="keyValues"> The keys to match on an entity. </param>
        /// <param name="options"> MongoDB find options. </param>
        /// <returns> The <typeparamref name="TEntity"/> matching the specified key values. </returns>
        public static TEntity Find<TEntity>(this IRepository<TEntity> repository, object[] keyValues, FindOptions options = null)
            where TEntity : class
        {
            var filter = Helpers.GetAndKeyFilter<TEntity>(keyValues);

            return Check(repository).Entities.Find(filter, options).Single();
        }

        /// <summary>
        ///     Finds a single <typeparamref name="TEntity"/> asynchronously.
        /// </summary>
        /// <typeparam name="TEntity"> Type of the entity to be added. </typeparam>
        /// <param name="repository"> <see cref="Repository{TEntity}"/> to add the entity to. </param>
        /// <param name="keyValues"> The keys to match on an entity. </param>
        /// <param name="options"> MongoDB find options. </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken" /> to observe while waiting for the task to complete. </param>
        /// <returns> A <see cref="Task"/> returning the <typeparamref name="TEntity"/> matching the specified key values. </returns>
        public static async Task<TEntity> FindAsync<TEntity>(this IRepository<TEntity> repository, object[] keyValues, FindOptions<TEntity, TEntity> options = null, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            var filter = Helpers.GetAndKeyFilter<TEntity>(keyValues);

            var findResult = await Check(repository).Entities.FindAsync(filter, options, cancellationToken);
            return await findResult.SingleAsync();
        }

        /// <summary>
        ///     Removes a single <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity"> Type of the entity to be added. </typeparam>
        /// <param name="repository"> <see cref="Repository{TEntity}"/> to add the entity to. </param>
        /// <param name="entity"> The <typeparamref name="TEntity"/> to be removed. </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken" /> to observe while waiting for the task to complete. </param>
        public static void Remove<TEntity>(this IRepository<TEntity> repository, TEntity entity, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            var result = Check(repository).Entities.DeleteOne(x => x == entity, cancellationToken);
            if (!result.IsAcknowledged)
                throw new Exception("The remove operation was not persisted");
        }

        /// <summary>
        ///     Removes a single <typeparamref name="TEntity"/> asynchronously.
        /// </summary>
        /// <typeparam name="TEntity"> Type of the entity to be added. </typeparam>
        /// <param name="repository"> <see cref="Repository{TEntity}"/> to add the entity to. </param>
        /// <param name="entity"> The <typeparamref name="TEntity"/> to be removed. </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken" /> to observe while waiting for the task to complete. </param>
        /// <returns> A <see cref="Task"/> representing the remove operation. </returns>
        public static async Task RemoveAsync<TEntity>(this IRepository<TEntity> repository, TEntity entity, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            var result = await Check(repository).Entities.DeleteOneAsync(x => x == entity, cancellationToken);
            if (!result.IsAcknowledged)
                throw new Exception("The remove operation was not persisted");
        }

        /// <summary>
        ///     Removes all <typeparamref name="TEntity"/>s.
        /// </summary>
        /// <typeparam name="TEntity"> Type of the entity to be added. </typeparam>
        /// <param name="repository"> <see cref="Repository{TEntity}"/> to add the entity to. </param>
        /// <param name="entities"> The <typeparamref name="TEntity"/>s to be removed. </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken" /> to observe while waiting for the task to complete. </param>
        public static void RemoveRange<TEntity>(this IRepository<TEntity> repository, IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            var result = Check(repository).Entities.DeleteMany(x => entities.Contains(x), cancellationToken);
            if (!result.IsAcknowledged)
                throw new Exception("The remove operation was not persisted");
        }

        /// <summary>
        ///     Removes all <typeparamref name="TEntity"/>s asynchronously.
        /// </summary>
        /// <typeparam name="TEntity"> Type of the entity to be added. </typeparam>
        /// <param name="repository"> <see cref="Repository{TEntity}"/> to add the entity to. </param>
        /// <param name="entities"> The <typeparamref name="TEntity"/>s to be removed. </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken" /> to observe while waiting for the task to complete. </param>
        /// <returns> A <see cref="Task"/> representing the remove operation. </returns>
        public static async Task RemoveRangeAsync<TEntity>(this IRepository<TEntity> repository, IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            var result = await Check(repository).Entities.DeleteManyAsync(x => entities.Contains(x), cancellationToken);
            if (!result.IsAcknowledged)
                throw new Exception("The remove operation was not persisted");
        }

        /// <summary>
        ///     Updates a single <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity"> Type of the entity to be added. </typeparam>
        /// <param name="repository"> <see cref="Repository{TEntity}"/> to add the entity to. </param>
        /// <param name="filter"> The definition to determine how entities will be filtered. </param>
        /// <param name="updateDefinition"> The definition to determine how entities will be updated. </param>
        /// <param name="options"> MongoDB update options. </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken" /> to observe while waiting for the task to complete. </param>
        public static void Update<TEntity>(this IRepository<TEntity> repository, FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> updateDefinition, UpdateOptions options = null, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            var result = Check(repository).Entities.UpdateOne(filter, updateDefinition, options, cancellationToken);
            if (!result.IsAcknowledged)
                throw new Exception("The update operation was not persisted");
        }

        /// <summary>
        ///     Updates a single <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity"> Type of the entity to be added. </typeparam>
        /// <param name="repository"> <see cref="Repository{TEntity}"/> to add the entity to. </param>
        /// <param name="filter"> The definition to determine how entities will be filtered. </param>
        /// <param name="updateDefinition"> The definition to determine how entities will be updated. </param>
        /// <param name="options"> MongoDB update options. </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken" /> to observe while waiting for the task to complete. </param>
        public static void Update<TEntity>(this IRepository<TEntity> repository, Expression<Func<TEntity, bool>> filter, UpdateDefinition<TEntity> updateDefinition, UpdateOptions options = null, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            var result = Check(repository).Entities.UpdateOne(filter, updateDefinition, options, cancellationToken);
            if (!result.IsAcknowledged)
                throw new Exception("The update operation was not persisted");
        }

        /// <summary>
        ///     Updates a single <typeparamref name="TEntity"/> asynchronously.
        /// </summary>
        /// <typeparam name="TEntity"> Type of the entity to be added. </typeparam>
        /// <param name="repository"> <see cref="Repository{TEntity}"/> to add the entity to. </param>
        /// <param name="filter"> The definition to determine how entities will be filtered. </param>
        /// <param name="updateDefinition"> The definition to determine how entities will be updated. </param>
        /// <param name="options"> MongoDB update options. </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken" /> to observe while waiting for the task to complete. </param>
        /// <returns> A <see cref="Task"/> representing the update operation. </returns>
        public static async Task UpdateAsync<TEntity>(this IRepository<TEntity> repository, FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> updateDefinition, UpdateOptions options = null, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            var result = await Check(repository).Entities.UpdateOneAsync(filter, updateDefinition, options, cancellationToken);
            if (!result.IsAcknowledged)
                throw new Exception("The update operation was not persisted");
        }

        /// <summary>
        ///     Updates a single <typeparamref name="TEntity"/> asynchronously.
        /// </summary>
        /// <typeparam name="TEntity"> Type of the entity to be added. </typeparam>
        /// <param name="repository"> <see cref="Repository{TEntity}"/> to add the entity to. </param>
        /// <param name="filter"> The definition to determine how entities will be filtered. </param>
        /// <param name="updateDefinition"> The definition to determine how entities will be updated. </param>
        /// <param name="options"> MongoDB update options. </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken" /> to observe while waiting for the task to complete. </param>
        /// <returns> A <see cref="Task"/> representing the update operation. </returns>
        public static async Task UpdateAsync<TEntity>(this IRepository<TEntity> repository, Expression<Func<TEntity, bool>> filter, UpdateDefinition<TEntity> updateDefinition, UpdateOptions options = null, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            var result = await Check(repository).Entities.UpdateOneAsync(filter, updateDefinition, options, cancellationToken);
            if (!result.IsAcknowledged)
                throw new Exception("The update operation was not persisted");
        }

        /// <summary>
        ///     Updates all <typeparamref name="TEntity"/>s.
        /// </summary>
        /// <typeparam name="TEntity"> Type of the entity to be added. </typeparam>
        /// <param name="repository"> <see cref="Repository{TEntity}"/> to add the entity to. </param>
        /// <param name="filter"> The definition to determine how entities will be filtered. </param>
        /// <param name="updateDefinition"> The definition to determine how entities will be updated. </param>
        /// <param name="options"> MongoDB update options. </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken" /> to observe while waiting for the task to complete. </param>
        public static void UpdateRange<TEntity>(this IRepository<TEntity> repository, FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> updateDefinition, UpdateOptions options = null, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            var result = Check(repository).Entities.UpdateMany(filter, updateDefinition, options, cancellationToken);
            if (!result.IsAcknowledged)
                throw new Exception("The update operation was not persisted");
        }

        /// <summary>
        ///     Updates all <typeparamref name="TEntity"/>s.
        /// </summary>
        /// <typeparam name="TEntity"> Type of the entity to be added. </typeparam>
        /// <param name="repository"> <see cref="Repository{TEntity}"/> to add the entity to. </param>
        /// <param name="filter"> The definition to determine how entities will be filtered. </param>
        /// <param name="updateDefinition"> The definition to determine how entities will be updated. </param>
        /// <param name="options"> MongoDB update options. </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken" /> to observe while waiting for the task to complete. </param>
        public static void UpdateRange<TEntity>(this IRepository<TEntity> repository, Expression<Func<TEntity, bool>> filter, UpdateDefinition<TEntity> updateDefinition, UpdateOptions options = null, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            var result = Check(repository).Entities.UpdateMany(filter, updateDefinition, options, cancellationToken);
            if (!result.IsAcknowledged)
                throw new Exception("The update operation was not persisted");
        }

        /// <summary>
        ///     Updates all <typeparamref name="TEntity"/>s asynchronously.
        /// </summary>
        /// <typeparam name="TEntity"> Type of the entity to be added. </typeparam>
        /// <param name="repository"> <see cref="Repository{TEntity}"/> to add the entity to. </param>
        /// <param name="filter"> The definition to determine how entities will be filtered. </param>
        /// <param name="updateDefinition"> The definition to determine how entities will be updated. </param>
        /// <param name="options"> MongoDB update options. </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken" /> to observe while waiting for the task to complete. </param>
        /// <returns> A <see cref="Task"/> representing the update operation. </returns>
        public static async Task UpdateRangeAsync<TEntity>(this IRepository<TEntity> repository, FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> updateDefinition, UpdateOptions options = null, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            var result = await Check(repository).Entities.UpdateManyAsync(filter, updateDefinition, options, cancellationToken);
            if (!result.IsAcknowledged)
                throw new Exception("The update operation was not persisted");
        }

        /// <summary>
        ///     Updates all <typeparamref name="TEntity"/>s asynchronously.
        /// </summary>
        /// <typeparam name="TEntity"> Type of the entity to be added. </typeparam>
        /// <param name="repository"> <see cref="Repository{TEntity}"/> to add the entity to. </param>
        /// <param name="filter"> The definition to determine how entities will be filtered. </param>
        /// <param name="updateDefinition"> The definition to determine how entities will be updated. </param>
        /// <param name="options"> MongoDB update options. </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken" /> to observe while waiting for the task to complete. </param>
        /// <returns> A <see cref="Task"/> representing the update operation. </returns>
        public static async Task UpdateRangeAsync<TEntity>(this IRepository<TEntity> repository, Expression<Func<TEntity, bool>> filter, UpdateDefinition<TEntity> updateDefinition, UpdateOptions options = null, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            var result = await Check(repository).Entities.UpdateManyAsync(filter, updateDefinition, options, cancellationToken);
            if (!result.IsAcknowledged)
                throw new Exception("The update operation was not persisted");
        }

        private static Repository<TEntity> Check<TEntity>(IRepository<TEntity> repository)
            where TEntity : class
        {
            if (!(repository is Repository<TEntity> mongoRepository))
                throw new InvalidOperationException(INVALID_REPO_MESSAGE);
            return mongoRepository;
        }
    }
}
