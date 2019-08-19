using System;
using JetBrains.Annotations;
using KG.DCX.Extensions.Cqrs;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDispatchers(
            [NotNull] this IServiceCollection services,
            [NotNull] Func<ServiceFactory> serviceFactory)
        {
            if (serviceFactory == null)
            {
                throw new ArgumentNullException(nameof(serviceFactory));
            }

            return services.AddDispatchers(_ => serviceFactory());
        }

        public static IServiceCollection AddDispatchers(
            [NotNull] this IServiceCollection services,
            [NotNull] Func<IServiceProvider, ServiceFactory> serviceFactory)
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
