using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MongoDB.Driver;

namespace KG.Data.MongoDB
{
    internal static class Helpers
    {
        public delegate FilterDefinition<TEntity> FilterAggregate<TEntity>(
            IEnumerable<FilterDefinition<TEntity>> filters)
            where TEntity : class;

        [UsedImplicitly]
        public static FilterDefinition<TEntity> GetKeyFilter<TEntity>(
            FilterAggregate<TEntity> aggregateFunction,
            params object[] keyValues)
            where TEntity : class
        {
            if (keyValues == null || keyValues.Any(x => x == null))
                return null;

            var properties = typeof(TEntity).GetProperties();

            IEnumerable<FilterDefinition<TEntity>> filters = keyValues
                .Select((t, i) => Builders<TEntity>.Filter
                    .Where(x => properties.Any(y => y.GetValue(x) == keyValues[i])));

            return aggregateFunction(filters);
        }

        public static FilterDefinition<TEntity> GetAndKeyFilter<TEntity>(params object[] keyValues)
            where TEntity : class
            => GetKeyFilter<TEntity>(Builders<TEntity>.Filter.And, keyValues);
    }
}
