using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace KG.DCX.Extensions.Cqrs
{
    internal class EventDispatcher : IEventDispatcher
    {
        private readonly ServiceFactory _serviceFactory;

        public EventDispatcher([NotNull] ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory ?? throw new ArgumentNullException(nameof(serviceFactory));
        }

        public Task DispatchAsync<T>([NotNull] T notification, CancellationToken cancellationToken = default)
            where T : IEvent
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            var handlers = _serviceFactory.GetRequiredService<IEnumerable<IEventHandler<T>>>();

            return Task.WhenAll(handlers.Select(h => h.HandleAsync(notification, cancellationToken)));
        }
    }
}
