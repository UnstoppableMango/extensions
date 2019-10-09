using UnMango.Extensions.Repository;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public const ServiceLifetime DEFAULT_LIFETIME = ServiceLifetime.Scoped;

        public static IServiceCollection AddRepository<TService, TImplementation>(
            this IServiceCollection services,
            ServiceLifetime lifetime = DEFAULT_LIFETIME)
            where TService : IRepository
            where TImplementation : TService
        {
            var descriptor = ServiceDescriptor.Describe(typeof(TService), typeof(TImplementation), lifetime);

            services.Add(descriptor);

            return services;
        }

        public static IServiceCollection AddRepository<TRepository, TEntity>(
            this IServiceCollection services,
            TRepository repository)
            where TRepository : IRepository<TEntity>
        {
            var descriptor = ServiceDescriptor.Singleton<IRepository<TEntity>>(repository);

            services.Add(descriptor);

            return services;
        }
    }
}
