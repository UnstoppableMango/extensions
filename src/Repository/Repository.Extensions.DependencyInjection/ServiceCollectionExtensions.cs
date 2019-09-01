using System;
using UnMango.Extensions.Repository;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds <typeparamref name="T"/> as an <see cref="IRepository"/> in the <see cref="IServiceCollection"/>.
        /// If <typeparamref name="T"/> imlements <see cref="IDisposable"/>, the repository will be registerd as
        /// <see cref="ServiceLifetime.Scoped"/>. Otherwise it will be registerd as <see cref="ServiceLifetime.Transient"/>.
        /// </summary>
        /// <typeparam name="T">The repository type to register.</typeparam>
        /// <param name="services">The collection to add registrations to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so calls can be chained.</returns>
        public static IServiceCollection AddRepository<T>(this IServiceCollection services)
            where T : class, IRepository
            => AddRepository(services, typeof(T));

        /// <summary>
        /// Adds <typeparamref name="T"/> as an <see cref="IRepository"/> in the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="T">The repository type to register.</typeparam>
        /// <param name="services">The collection to add registrations to.</param>
        /// <param name="lifetime">The <see cref="ServiceLifetime"/> to use for the registration.</param>
        /// <returns>The <see cref="IServiceCollection"/> so calls can be chained.</returns>
        public static IServiceCollection AddRepository<T>(this IServiceCollection services, ServiceLifetime lifetime)
            where T : class, IRepository
            => AddRepository(services, typeof(T), lifetime);

        /// <summary>
        /// Adds <paramref name="implementationType"/> as an <see cref="IRepository"/> in the <see cref="IServiceCollection"/>.
        /// If <paramref name="implementationType"/> imlements <see cref="IDisposable"/>, the repository will be registerd as
        /// <see cref="ServiceLifetime.Scoped"/>. Otherwise it will be registerd as <see cref="ServiceLifetime.Transient"/>.
        /// If <paramref name="implementationType"/> does not implement <see cref="IRepository"/>, an exception will be thrown.
        /// </summary>
        /// <param name="services">The collection to add registrations to.</param>
        /// <param name="implementationType">The <see cref="Type"/> to register as an <see cref="IRepository"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> so calls can be chained.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="implementationType"/> does not implement <see cref="IRepository"/>.
        /// </exception>
        public static IServiceCollection AddRepository(this IServiceCollection services, Type implementationType)
            => AddRepository(services, implementationType, GetLifetime(implementationType));

        /// <summary>
        /// Adds <paramref name="implementationType"/> as an <see cref="IRepository"/> in the <see cref="IServiceCollection"/>.
        /// If <paramref name="implementationType"/> does not implement <see cref="IRepository"/>, an exception will be thrown.
        /// </summary>
        /// <param name="services">The collection to add registrations to.</param>
        /// <param name="implementationType">The <see cref="Type"/> to register as an <see cref="IRepository"/>.</param>
        /// <param name="lifetime">The <see cref="ServiceLifetime"/> to use for the registration.</param>
        /// <returns>The <see cref="IServiceCollection"/> so calls can be chained.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="implementationType"/> does not implement <see cref="IRepository"/>
        /// or when <paramref name="implementationType"/> is not concrete.
        /// </exception>
        public static IServiceCollection AddRepository(this IServiceCollection services, Type implementationType, ServiceLifetime lifetime)
        {
            if (!implementationType.IsConcrete())
                throw new ArgumentException("The type is not a concrete type", nameof(implementationType));

            if (!RepoUtility.ImplementsIRepository(implementationType))
                throw new ArgumentException($"The type does not implement {nameof(IRepository)}", nameof(implementationType));

            services.Add(new ServiceDescriptor(implementationType, implementationType, lifetime));
            services.Add(new ServiceDescriptor(typeof(IRepository), x => x.GetRequiredService(implementationType), lifetime));

            if (RepoUtility.ImplementsGenericIRepository(implementationType))
            {
                var entityType = RepoUtility.GetEntityType(implementationType);
                var serviceType = typeof(IRepository<>).MakeGenericType(entityType);
                services.Add(new ServiceDescriptor(serviceType, x => x.GetRequiredService(implementationType), lifetime));
            }

            return services;
        }

        /// <summary>
        /// Adds <typeparamref name="T"/> as an <see cref="IUnitOfWork"/> in the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="T">The unit of work type to register.</typeparam>
        /// <param name="services">The collection to add registrations to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so calls can be chained.</returns>
        public static IServiceCollection AddUnitOfWork<T>(this IServiceCollection services)
            where T : class, IUnitOfWork
            => services.AddUnitOfWork(typeof(T));

        /// <summary>
        /// Adds <paramref name="implementationType"/> as an <see cref="IUnitOfWork"/> in the <see cref="IServiceCollection"/>.
        /// If <paramref name="implementationType"/> does not implement <see cref="IUnitOfWork"/>, an exception will be thrown.
        /// </summary>
        /// <param name="services">The collection to add registrations to.</param>
        /// <param name="implementationType">The <see cref="Type"/> to register as an <see cref="IUnitOfWork"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> so calls can be chained.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="implementationType"/> does not implement <see cref="IUnitOfWork"/>
        /// or when <paramref name="implementationType"/> is not concrete.
        /// </exception>
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services, Type implementationType)
        {
            if (!implementationType.IsConcrete())
                throw new ArgumentException("The type is not a concrete type", nameof(implementationType));

            if (!typeof(IUnitOfWork).IsAssignableFrom(implementationType))
                throw new ArgumentException($"The type does not implement {nameof(IUnitOfWork)}", nameof(implementationType));

            services.AddScoped(implementationType, implementationType);
            services.AddScoped(typeof(IUnitOfWork), x => x.GetRequiredService(implementationType));

            return services;
        }

        private static ServiceLifetime GetLifetime(Type type)
            => typeof(IDisposable).IsAssignableFrom(type)
                ? ServiceLifetime.Scoped
                : ServiceLifetime.Transient;
    }
}
