using System.Threading;
using System.Threading.Tasks;

namespace KG.DCX.Extensions.Cqrs
{
    /// <summary>
    /// Dispatches events to all injected handlers.
    /// </summary>
    public interface IEventDispatcher
    {
        /// <summary>
        /// Dispatches an event of type <typeparamref name="T"/> to injected handlers.
        /// </summary>
        /// <typeparam name="T">The type of event dispatched.</typeparam>
        /// <param name="notification">The event to be dispatched.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>A <see cref="Task"/> representing the operation.</returns>
        Task DispatchAsync<T>(T notification, CancellationToken cancellationToken = default)
            where T : IEvent;
    }
}
