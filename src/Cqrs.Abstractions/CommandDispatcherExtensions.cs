using System;
using System.Threading;
using System.Threading.Tasks;

namespace KG.DCX.Extensions.Cqrs
{
    public static class CommandDispatcherExtensions
    {
        public static Task<T> DispatchAsync<T>(
            this ICommandDispatcher dispatcher,
            ICommand<T> request,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
