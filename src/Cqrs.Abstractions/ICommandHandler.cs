using System.Threading;
using System.Threading.Tasks;

namespace KG.DCX.Extensions.Cqrs
{
    /// <summary>
    /// Handles commands of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of command handled.</typeparam>
    public interface ICommandHandler<in T> : ICommandHandler
        where T : ICommand
    {
        /// <summary>
        /// Handles a command of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="request">The command to handle.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>A <see cref="Task"/> representing the operation.</returns>
        Task HandleAsync(T request, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Handles commands of type <typeparamref name="TCommand"/> that return <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TCommand">The type of command handled.</typeparam>
    /// <typeparam name="TResult">The commands return value.</typeparam>
    public interface ICommandHandler<in TCommand, TResult> : ICommandHandler
        where TCommand : ICommand<TResult>
    {
        /// <summary>
        /// Handles a command of type <typeparamref name="TCommand"/> that returns a <typeparamref name="TResult"/>.
        /// </summary>
        /// <param name="request">The command to handle.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>A <see cref="Task"/> representing the operation.</returns>
        Task<TResult> HandleAsync(TCommand request, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Defines an interface for all command handlers
    /// </summary>
    public interface ICommandHandler { }
}
