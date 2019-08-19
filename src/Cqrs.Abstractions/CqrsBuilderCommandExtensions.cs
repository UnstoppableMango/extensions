using System;
using System.Linq;
using KG.DCX.Extensions.Cqrs;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for working with commands on a <see cref="CqrsBuilder"/>.
    /// </summary>
    public static class CqrsBuilderCommandExtensions
    {
        private static readonly Type _nonValueReturningServiceType = typeof(ICommandHandler<>);
        private static readonly Type _valueReturningServiceType = typeof(ICommandHandler<,>);

        /// <summary>
        /// Adds a command handler of type <typeparamref name="THandler"/> that handles
        /// <typeparamref name="TCommand"/>s that return <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="THandler">The type of the handler to add.</typeparam>
        /// <typeparam name="TCommand">The type of command the handler handles.</typeparam>
        /// <typeparam name="TResult">The type the command returns.</typeparam>
        /// <param name="builder">The builder to add the handler to.</param>
        /// <param name="handlerLifetime">The service lifetime for the handler.</param>
        /// <returns>The <see cref="CqrsBuilder"/> so calls can be chained.</returns>
        public static CqrsBuilder AddCommandHandler<THandler, TCommand, TResult>(
            this CqrsBuilder builder,
            ServiceLifetime handlerLifetime = CqrsBuilder.DEFAULT_HANDLER_LIFETIME)
            where THandler : class, ICommandHandler<TCommand, TResult>
            where TCommand : ICommand<TResult>
        {
            return AddCommandHandlerCore(builder, typeof(THandler), handlerLifetime);
        }

        /// <summary>
        /// Adds a command handler of type <typeparamref name="THandler"/> that handles
        /// <typeparamref name="TCommand"/>s.
        /// </summary>
        /// <typeparam name="THandler">The type of the handler to add.</typeparam>
        /// <typeparam name="TCommand">The type of command the handler handles.</typeparam>
        /// <param name="builder">The builder to add the handler to.</param>
        /// <param name="handlerLifetime">The service lifetime for the handler.</param>
        /// <returns>The <see cref="CqrsBuilder"/> so calls can be chained.</returns>
        public static CqrsBuilder AddCommandHandler<THandler, TCommand>(
            this CqrsBuilder builder,
            ServiceLifetime handlerLifetime = CqrsBuilder.DEFAULT_HANDLER_LIFETIME)
            where THandler : class, ICommandHandler<TCommand>
            where TCommand : ICommand
        {
            return AddCommandHandlerCore(builder, typeof(THandler), handlerLifetime);
        }

        /// <summary>
        /// Adds a command handler of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the handler to add.</typeparam>
        /// <param name="builder">The builder to add the handler to.</param>
        /// <param name="handlerLifetime">The service lifetime for the handler.</param>
        /// <returns>The <see cref="CqrsBuilder"/> so calls can be chained.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <typeparamref name="T"/> is not a valid command handler.
        /// </exception>
        /// <remarks>
        /// The explicit generic methods are preferred over this since they provide
        /// compile time type checking, rather than runtime.
        /// </remarks>
        public static CqrsBuilder AddCommandHandler<T>(
            this CqrsBuilder builder,
            ServiceLifetime handlerLifetime = CqrsBuilder.DEFAULT_HANDLER_LIFETIME)
            where T : ICommandHandler
        {
            return AddCommandHandlerCore(builder, typeof(T), handlerLifetime);
        }

        /// <summary>
        /// Adds a command handler of type <paramref name="handlerType"/>.
        /// </summary>
        /// <param name="builder">The builder to add the handler to.</param>
        /// <param name="handlerType">The type of the handler to add.</param>
        /// <param name="handlerLifetime">The service lifetime for the handler.</param>
        /// <returns>The <see cref="CqrsBuilder"/> so calls can be chained.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="handlerType"/> is not a valid command handler.
        /// </exception>
        /// <remarks>
        /// The explicit generic methods are preferred over this since they provide
        /// compile time type checking, rather than runtime.
        /// </remarks>
        public static CqrsBuilder AddCommandHandler(
            this CqrsBuilder builder,
            Type handlerType,
            ServiceLifetime handlerLifetime = CqrsBuilder.DEFAULT_HANDLER_LIFETIME)
        {
            return AddCommandHandlerCore(builder, handlerType, handlerLifetime);
        }

        private static CqrsBuilder AddCommandHandlerCore(CqrsBuilder builder, Type handlerType, ServiceLifetime handlerLifetime)
        {
            return builder.AddHandlerCore(GetCommandHandlerServiceType, handlerType, handlerLifetime);
        }

        private static Type GetCommandHandlerServiceType(Type handlerType)
        {
            var handlerInterface = Array.Find(handlerType.GetInterfaces(), IsCommandHandlerInterface);

            if (handlerInterface == null) throw new ArgumentException($"{handlerType} is not a valid command handler.");

            var genericArguments = handlerInterface.GetGenericArguments();
            var commandType = genericArguments[0];
            var resultType = genericArguments.ElementAtOrDefault(1);

            return resultType == null
                ? _nonValueReturningServiceType.MakeGenericType(commandType)
                : _valueReturningServiceType.MakeGenericType(commandType, resultType);
        }

        private static bool IsCommandHandlerInterface(Type t)
        {
            if (!t.IsInterface) return false;

            try
            {
                var genericDefinition = t.GetGenericTypeDefinition();

                return typeof(ICommandHandler<>).IsAssignableFrom(genericDefinition)
                    || typeof(ICommandHandler<,>).IsAssignableFrom(genericDefinition);
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
    }
}
