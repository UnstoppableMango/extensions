using System.Threading;
using System.Threading.Tasks;

namespace KG.DCX.Extensions.Cqrs
{
    /// <summary>
    /// Dispatches commands to their respective handlers.
    /// </summary>
    public interface ICommandDispatcher
    {
        /// <summary>
        /// Dispatches a command of type <typeparamref name="T"/> to its handler.
        /// </summary>
        /// <typeparam name="T">The type of command dispatched.</typeparam>
        /// <param name="request">The command to be dispatched.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>A <see cref="Task"/> representing the operation.</returns>
        Task DispatchAsync<T>(T request, CancellationToken cancellationToken = default)
            where T : ICommand;

        /// <summary>
        /// Dispatches a command of type <typeparamref name="T"/> to its handler and returns the result.
        /// </summary>
        /// <typeparam name="T">The return type of the dispatched command.</typeparam>
        /// <param name="request">The command to be dispatched.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>A <see cref="Task"/> representing the operation.</returns>
        Task<T> DispatchAsync<T>(ICommand<T> request, CancellationToken cancellationToken = default);
    }
}
