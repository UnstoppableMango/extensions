using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KG.DCX.Extensions.Cqrs
{
    internal class EventDispatcher : IEventDispatcher
    {
        private readonly ServiceFactory _serviceFactory;

        public EventDispatcher(ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public Task DispatchAsync<T>(T notification, CancellationToken cancellationToken = default)
            where T : IEvent
        {
            var handlers = _serviceFactory.GetRequiredService<IEnumerable<IEventHandler<T>>>();

            return Task.WhenAll(handlers.Select(h => h.HandleAsync(notification, cancellationToken)));
        }
    }
}
