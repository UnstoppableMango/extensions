using System;

namespace UnMango.Extensions.Repository
{
    /// <summary>
    /// Extension methods for <see cref="IUnitOfWork"/>.
    /// </summary>
    public static class UnitOfWorkExtensions
    {
        private const string INVALID_UOW_MESSAGE = "Underlying unit of work is not a UnitOfWorkBase";

        /// <summary>
        /// Gets the <see cref="IRepository{TEntity}"/> for the specified <typeparamref name="TEntity"/>.
        /// Will throw an exception if <paramref name="unitOfWork"/> does not implement <see cref="UnitOfWorkBase"/>.
        /// </summary>
        /// <typeparam name="TEntity">The <see cref="IRepository{TEntity}"/> to retrieve.</typeparam>
        /// <param name="unitOfWork">The <see cref="IUnitOfWork"/> scope to use.</param>
        /// <returns>The <see cref="IRepository{TEntity}"/> for the specifed entity type.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when <paramref name="unitOfWork"/> does not implement <see cref="UnitOfWorkBase"/>.
        /// </exception>
        [Obsolete("Use the method from the providers package")]
        public static IRepository<TEntity>? Entities<TEntity>(this IUnitOfWork unitOfWork)
            where TEntity : class
            => Check(unitOfWork).Repository<TEntity>();

        private static UnitOfWorkBase Check(IUnitOfWork unitOfWork)
        {
            if (!(unitOfWork is UnitOfWorkBase unitOfWorkBase))
                throw new InvalidOperationException(INVALID_UOW_MESSAGE);

            return unitOfWorkBase;
        }
    }
}
