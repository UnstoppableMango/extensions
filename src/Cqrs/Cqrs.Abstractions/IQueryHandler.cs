using System.Threading;
using System.Threading.Tasks;

namespace KG.DCX.Extensions.Cqrs
{
    /// <summary>
    /// Handles queries of type <typeparamref name="TQuery"/> that return <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TQuery">The type of query handled.</typeparam>
    /// <typeparam name="TResult">The type the query returns.</typeparam>
    public interface IQueryHandler<in TQuery, TResult> : IQueryHandler
        where TQuery : IQuery<TResult>
    {
        /// <summary>
        /// Handles a query of type <typeparamref name="TQuery"/> that returns a <typeparamref name="TResult"/>.
        /// </summary>
        /// <param name="request">The query to handle.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>A <see cref="Task"/> representing the operation.</returns>
        Task<TResult> HandleAsync(TQuery request, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Defines an interface for all query handlers.
    /// </summary>
    public interface IQueryHandler { }
}
