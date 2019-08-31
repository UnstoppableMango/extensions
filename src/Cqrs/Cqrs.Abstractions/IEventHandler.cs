using System.Threading;
using System.Threading.Tasks;

namespace KG.DCX.Extensions.Cqrs
{
    /// <summary>
    /// Handles events of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of event handled.</typeparam>
    public interface IEventHandler<in T> : IEventHandler
        where T : IEvent
    {
        /// <summary>
        /// Handle an event of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="notification">The event to handle.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>A <see cref="Task"/> representing the operation.</returns>
        Task HandleAsync(T notification, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Defines an interface for all event handlers.
    /// </summary>
    public interface IEventHandler { }
}
