using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace UnMango.Extensions.Repository
{
    /// <summary>
    ///     An implementation of the unit of work pattern.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        ///     Persists the changes made with the unit of work.
        /// </summary>
        /// <returns> Whether the action was successful or not. </returns>
        [UsedImplicitly]
        bool SaveChanges();

        /// <summary>
        ///     Persists the changes made with the unit of work asynchronously.
        /// </summary>
        /// <returns> Whether the action was successful or not. </returns>
        [UsedImplicitly]
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
