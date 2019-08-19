using System;
using KG.DCX.Extensions.Cqrs;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDispatchers(
            this IServiceCollection services,
            Func<ServiceFactory> serviceFactory)
        {
            if (serviceFactory == null)
            {
                throw new ArgumentNullException(nameof(serviceFactory));
            }

            return services.AddDispatchers(_ => serviceFactory());
        }

        public static IServiceCollection AddDispatchers(
            this IServiceCollection services,
            Func<IServiceProvider, ServiceFactory> serviceFactory)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (serviceFactory == null)
            {
                throw new ArgumentNullException(nameof(serviceFactory));
            }

            services.AddSingleton(serviceFactory);
            services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
            services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
            services.AddSingleton<IEventDispatcher, EventDispatcher>();

            return services;
        }
    }
}
