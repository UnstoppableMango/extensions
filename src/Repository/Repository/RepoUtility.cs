using System;
using System.Linq;
using JetBrains.Annotations;

namespace UnMango.Extensions.Repository
{
    /// <summary>
    /// Utility class for <see cref="IRepository"/>s.
    /// </summary>
    public static class RepoUtility
    {
        /// <summary>
        ///     Determines whether the specified type implements <see cref="IRepository"/>.
        /// </summary>
        /// <param name="type"> The type to check. </param>
        /// <returns> Whether the type implements <see cref="IRepository"/> or not. </returns>
        public static bool ImplementsIRepository([CanBeNull] Type type) => typeof(IRepository).IsAssignableFrom(type);

        /// <summary>
        ///     Determines whether the specified object implements <see cref="IRepository"/>.
        /// </summary>
        /// <param name="obj"> The object to check. </param>
        /// <returns> Whether the object implements <see cref="IRepository"/> or not. </returns>
        public static bool ImplementsIRepository([NotNull] object obj)
            => ImplementsIRepository(Check.NotNull(obj, nameof(obj)).GetType());

        /// <summary>
        ///     Determines whether the specified type implements <see cref="IRepository{TEntity}"/>.
        /// </summary>
        /// <param name="type"> The type to check. </param>
        /// <returns> Whether the type is <see cref="IRepository{TEntity}"/> or not. </returns>
        public static bool IsGenericRepositoryType([NotNull] Type type)
        {
            Check.NotNull(type, nameof(type));

            if (!type.IsGenericType)
                return false;

            return type.GetGenericTypeDefinition() == typeof(IRepository<>);
        }

        /// <summary>
        ///     Determines whether the specified type is <see cref="IRepository{TEntity}"/>.
        /// </summary>
        /// <param name="type"> The type to check. </param>
        /// <returns> Whether the type implements <see cref="IRepository{TEntity}"/> or not. </returns>
        public static bool ImplementsGenericIRepository([NotNull] Type type)
        {
            Check.NotNull(type, nameof(type));

            if (IsGenericRepositoryType(type))
                return true;

            var interfaces = type.GetInterfaces();

            if (interfaces.Any(IsGenericRepositoryType))
                return true;

            var baseType = type.BaseType;

            return baseType != null && ImplementsGenericIRepository(baseType);
        }

        /// <summary>
        ///     Determines whether the specified object is <see cref="IRepository{TEntity}"/>.
        /// </summary>
        /// <param name="obj"> The object to check. </param>
        /// <returns> Whether the object implements <see cref="IRepository{TEntity}"/> or not. </returns>
        public static bool ImplementsGenericIRepository([NotNull] object obj)
            => ImplementsGenericIRepository(Check.NotNull(obj, nameof(obj)).GetType());

        /// <summary>
        /// If <paramref name="type"/> is an <see cref="IRepository{TEntity}"/>, gets the
        /// entity type. Otherwise return null.
        /// </summary>
        /// <param name="type">The repository type to get the entity type of.</param>
        /// <returns>The repository's entity type, or null.</returns>
        public static Type? GetEntityType([NotNull] Type type)
        {
            Check.NotNull(type, nameof(type));

            var interfaces = type.GetInterfaces();

            var repositoryType = interfaces.FirstOrDefault(IsGenericRepositoryType);

            return repositoryType?.GetGenericArguments()[0];
        }
    }
}
