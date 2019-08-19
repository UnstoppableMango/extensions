using System;
using JetBrains.Annotations;
using KG.DCX.Extensions.Cqrs;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for working with events on a <see cref="CqrsBuilder"/>.
    /// </summary>
    public static class CqrsBuilderEventExtensions
    {
        private static readonly Type _serviceType = typeof(IEventHandler<>);

        /// <summary>
        /// Adds an event handler of type <typeparamref name="THandler"/> that handles
        /// <typeparamref name="TEvent"/>s.
        /// </summary>
        /// <typeparam name="THandler">The type of the handler to add.</typeparam>
        /// <typeparam name="TEvent">The type of the event that the handler handles.</typeparam>
        /// <param name="builder">The builder to add the handler to.</param>
        /// <param name="handlerLifetime">The service lifetime for the handler.</param>
        /// <returns>The <see cref="CqrsBuilder"/> so calls can be chained.</returns>
        public static CqrsBuilder AddEventHandler<THandler, TEvent>(
            this CqrsBuilder builder,
            ServiceLifetime handlerLifetime = CqrsBuilder.DEFAULT_HANDLER_LIFETIME)
            where THandler : IEventHandler<TEvent>
            where TEvent : IEvent
        {
            return AddEventHandlerCore(builder, typeof(THandler), handlerLifetime);
        }

        /// <summary>
        /// Adds an event handler of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the handler to add.</typeparam>
        /// <param name="builder">The builder to add the handler to.</param>
        /// <param name="handlerLifetime">The service lifetime for the handler.</param>
        /// <returns>The <see cref="CqrsBuilder"/> so calls can be chained.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <typeparamref name="T"/> is not a valid event handler.
        /// </exception>
        /// <remarks>
        /// The explicit generic methods are preferred over this since they provide
        /// compile time type checking, rather than runtime.
        /// </remarks>
        public static CqrsBuilder AddEventHandler<T>(
            this CqrsBuilder builder,
            ServiceLifetime handlerLifetime = CqrsBuilder.DEFAULT_HANDLER_LIFETIME)
            where T : IEventHandler
        {
            return AddEventHandlerCore(builder, typeof(T), handlerLifetime);
        }

        /// <summary>
        /// Adds an event handler of type <paramref name="handlerType"/>.
        /// </summary>
        /// <param name="builder">The builder to add the handler to.</param>
        /// <param name="handlerType">The type of the handler to add.</param>
        /// <param name="handlerLifetime">The service lifetime for the handler.</param>
        /// <returns>The <see cref="CqrsBuilder"/> so calls can be chained.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="handlerType"/> is not a valid event handler.
        /// </exception>
        /// <remarks>
        /// The explicit generic methods are preferred over this since they provide
        /// compile time type checking, rather than runtime.
        /// </remarks>
        public static CqrsBuilder AddEventHandler(
            this CqrsBuilder builder,
            Type handlerType,
            ServiceLifetime handlerLifetime = CqrsBuilder.DEFAULT_HANDLER_LIFETIME)
        {
            return AddEventHandlerCore(builder, handlerType, handlerLifetime);
        }

        private static CqrsBuilder AddEventHandlerCore(
            [NotNull] CqrsBuilder builder,
            [NotNull] Type handlerType,
            ServiceLifetime handlerLifetime)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (handlerType == null) throw new ArgumentNullException(nameof(handlerType));

            return builder.AddHandlerCore(GetEventHandlerServiceType, handlerType, handlerLifetime);
        }

        private static Type GetEventHandlerServiceType(Type handlerType)
        {
            var handlerInterface = Array.Find(handlerType.GetInterfaces(), IsEventHandlerInterface);

            if (handlerInterface == null) throw new ArgumentException($"{handlerType} is not a valid event handler.");

            var genericArguments = handlerInterface.GetGenericArguments();
            var eventType = genericArguments[0];

            return _serviceType.MakeGenericType(eventType);
        }

        private static bool IsEventHandlerInterface(Type t)
        {
            if (!t.IsInterface) return false;

            try
            {
                var genericDefinition = t.GetGenericTypeDefinition();

                return typeof(IEventHandler<>).IsAssignableFrom(genericDefinition);
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
    }
}
