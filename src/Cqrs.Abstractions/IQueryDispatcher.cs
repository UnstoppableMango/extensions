using System.Threading;
using System.Threading.Tasks;

namespace KG.DCX.Extensions.Cqrs
{
    /// <summary>
    /// Dispatches queries to their respective handlers.
    /// </summary>
    public interface IQueryDispatcher
    {
        /// <summary>
        /// Dispatches a query that returns <typeparamref name="T"/> to its handler.
        /// </summary>
        /// <typeparam name="T">The type the dispatched query will return.</typeparam>
        /// <param name="request">The query to be dispatched.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>A <see cref="Task"/> representing the operation.</returns>
        Task<T> DispatchAsync<T>(IQuery<T> request, CancellationToken cancellationToken = default);
    }
}
