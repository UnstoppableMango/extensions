using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace KG.DCX.Extensions.Cqrs
{
    internal class QueryDispatcher : IQueryDispatcher
    {
        private readonly ServiceFactory _serviceFactory;

        public QueryDispatcher(ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public Task<TResult> DispatchAsync<TQuery, TResult>(TQuery request, CancellationToken cancellationToken = default)
            where TQuery : IQuery<TResult>
        {
            var handler = _serviceFactory.GetRequiredService<IQueryHandler<TQuery, TResult>>();

            return handler.HandleAsync(request, cancellationToken);
        }

        public Task<T> DispatchAsync<T>(IQuery<T> request, CancellationToken cancellationToken = default)
        {
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
