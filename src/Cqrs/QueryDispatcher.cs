using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace KG.DCX.Extensions.Cqrs
{
    internal class QueryDispatcher : IQueryDispatcher
    {
        private readonly ServiceFactory _serviceFactory;

        public QueryDispatcher([NotNull] ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory ?? throw new ArgumentNullException(nameof(serviceFactory));
        }

        public Task<TResult> DispatchAsync<TQuery, TResult>([NotNull] TQuery request, CancellationToken cancellationToken = default)
            where TQuery : IQuery<TResult>
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var handler = _serviceFactory.GetRequiredService<IQueryHandler<TQuery, TResult>>();

            return handler.HandleAsync(request, cancellationToken);
        }

        public Task<T> DispatchAsync<T>([NotNull] IQuery<T> request, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var handlerType = GetHandlerType(request.GetType(), typeof(T));
            var handler = _serviceFactory(handlerType);

            var handleMethod = handlerType.GetMethod("HandleAsync");

            try
            {
                var parameters = new object[] { request, cancellationToken };
                var result = handleMethod.Invoke(handler, parameters);
                return (Task<T>)result;
            }
            catch (TargetInvocationException e)
            {
                // Unwrap exception from reflection
                throw e.InnerException;
            }
        }

        private static Type GetHandlerType(Type queryType, Type resultType)
        {
            var openHandler = typeof(IQueryHandler<,>);

            return openHandler.MakeGenericType(queryType, resultType);
        }
    }
}
