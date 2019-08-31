using System;
using System.Linq;
using JetBrains.Annotations;

namespace KG.Data
{
    public static class RepositoryExtensions
    {
        /// <summary>
        /// Gets the underlying entity type for the <paramref name="repository"/>.
        /// </summary>
        /// <param name="repository">The <see cref="IRepository"/> to get the entity type of.</param>
        /// <returns>The underlying entity <see cref="Type"/>.</returns>
        public static Type EntityType([NotNull] this IRepository repository)
            => repository
                .GetType()
                .GetInterfaces()
                .FirstOrDefault(RepoUtility.IsGenericRepositoryType)
                ?.GetGenericArguments()[0];

        /// <summary>
        /// Gets the underlying entity type for the <paramref name="repository"/>.
        /// </summary>
        /// <typeparam name="TEntity">The underlying entity type.</typeparam>
        /// <param name="repository">The <see cref="IRepository{TEntity}"/> to get the entity type of.</param>
        /// <returns>The underlying entity <see cref="Type"/>.</returns>
        [UsedImplicitly]
        public static Type EntityType<TEntity>([CanBeNull] this IRepository<TEntity> repository)
            where TEntity : class
            => typeof(TEntity);
    }
}
