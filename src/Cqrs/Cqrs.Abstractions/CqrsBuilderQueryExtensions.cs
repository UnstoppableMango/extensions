using System;
using KG.DCX.Extensions.Cqrs;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for working with queries on a <see cref="CqrsBuilder"/>.
    /// </summary>
    public static class CqrsBuilderQueryExtensions
    {
        private static readonly Type _serviceType = typeof(IQueryHandler<,>);

        /// <summary>
        /// Adds a query handler of type <typeparamref name="THandler"/> that handles
        /// <typeparamref name="TQuery"/>s that return <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="THandler">The type of handler to add.</typeparam>
        /// <typeparam name="TQuery">The type of query the handler handles.</typeparam>
        /// <typeparam name="TResult">The type the query returns.</typeparam>
        /// <param name="builder">The builder to add the handler to.</param>
        /// <param name="handlerLifetime">The service lifetime for the handler.</param>
        /// <returns>The <see cref="CqrsBuilder"/> so calls can be chained.</returns>
        public static CqrsBuilder AddQueryHandler<THandler, TQuery, TResult>(
            this CqrsBuilder builder,
            ServiceLifetime handlerLifetime = CqrsBuilder.DEFAULT_HANDLER_LIFETIME)
            where THandler : class, IQueryHandler<TQuery, TResult>
            where TQuery : IQuery<TResult>
        {
            return AddQueryHandlerCore(builder, typeof(THandler), handlerLifetime);
        }

        /// <summary>
        /// Adds a query handler of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of handler to add.</typeparam>
        /// <param name="builder">The builder to add the handler to.</param>
        /// <param name="handlerLifetime">The service lifetime for the handler.</param>
        /// <returns>The <see cref="CqrsBuilder"/> so calls can be chained.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <typeparamref name="T"/> is not a valid query handler.
        /// </exception>
        /// <remarks>
        /// The explicit generic methods are preferred over this since they provide
        /// compile time type checking, rather than runtime.
        /// </remarks>
        public static CqrsBuilder AddQueryHandler<T>(
            this CqrsBuilder builder,
            ServiceLifetime handlerLifetime = CqrsBuilder.DEFAULT_HANDLER_LIFETIME)
            where T : IQueryHandler
        {
            return AddQueryHandlerCore(builder, typeof(T), handlerLifetime);
        }

        /// <summary>
        /// Adds a query handler of type <paramref name="handlerType"/>.
        /// </summary>
        /// <param name="builder">The builder to add the handler to.</param>
        /// <param name="handlerType">The type of the handler to add.</param>
        /// <param name="handlerLifetime">The service lifetime for the handler.</param>
        /// <returns>The <see cref="CqrsBuilder"/> so calls can be chained.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="handlerType"/> is not a valid query handler.
        /// </exception>
        /// <remarks>
        /// The explicit generic methods are preferred over this since they provide
        /// compile time type checking, rather than runtime.
        /// </remarks>
        public static CqrsBuilder AddQueryHandler(
            this CqrsBuilder builder,
            Type handlerType,
            ServiceLifetime handlerLifetime = CqrsBuilder.DEFAULT_HANDLER_LIFETIME)
        {
            return AddQueryHandlerCore(builder, handlerType, handlerLifetime);
        }

        private static CqrsBuilder AddQueryHandlerCore(CqrsBuilder builder, Type handlerType, ServiceLifetime handlerLifetime)
        {
            return builder.AddHandlerCore(GetQueryHandlerServiceType, handlerType, handlerLifetime);
        }

        private static Type GetQueryHandlerServiceType(Type handlerType)
        {
            var handlerInterface = Array.Find(handlerType.GetInterfaces(), IsQueryHandlerInterface);

            if (handlerInterface == null) throw new ArgumentException($"{handlerType} is not a valid query handler.");

            var genericArguments = handlerInterface.GetGenericArguments();
            var queryType = genericArguments[0];
            var resultType = genericArguments[1];

            return _serviceType.MakeGenericType(queryType, resultType);
        }

        private static bool IsQueryHandlerInterface(Type t)
        {
            if (!t.IsInterface) return false;

            try
            {
                var genericDefinition = t.GetGenericTypeDefinition();

                return typeof(IQueryHandler<,>).IsAssignableFrom(genericDefinition);
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
    }
}
