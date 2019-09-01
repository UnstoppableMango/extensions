using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using UnMango.Extensions.Repository;
using UnMango.Extensions.Repository.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        [UsedImplicitly]
        public static IServiceCollection AddUnitOfWork(
            [NotNull] this IServiceCollection services,
            [CanBeNull] Action<DbContextOptionsBuilder>? optionsAction,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
            ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
            => AddUnitOfWork<UnitOfWork>(services, optionsAction, contextLifetime, optionsLifetime);

        [UsedImplicitly]
        public static IServiceCollection AddUnitOfWork<T>(
            [NotNull] this IServiceCollection services,
            [CanBeNull] Action<DbContextOptionsBuilder>? optionsAction,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
            ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
            where T : UnitOfWork
            => AddUnitOfWork<T>(
                services,
                optionsAction == null
                    ? (Action<IServiceProvider, DbContextOptionsBuilder>?)null
                    : (p, b) => optionsAction?.Invoke(b),
                contextLifetime,
                optionsLifetime);

        [UsedImplicitly]
        public static IServiceCollection AddUnitOfWork<T>(
            [NotNull] this IServiceCollection services,
            [CanBeNull] Action<IServiceProvider, DbContextOptionsBuilder>? optionsAction,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
            ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
            where T : UnitOfWork
            => AddUnitOfWork<T, T>(services, optionsAction, contextLifetime, optionsLifetime);

        [UsedImplicitly]
        public static IServiceCollection AddUnitOfWork<TService, TUnitOfWork>(
            [NotNull] this IServiceCollection services,
            [CanBeNull] Action<IServiceProvider, DbContextOptionsBuilder>? optionsAction,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
            ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
            where TUnitOfWork : UnitOfWork, TService
        {
            Check.NotNull(services, nameof(services));

            services.AddUnitOfWork<TUnitOfWork>();

            services.AddDbContext<TService, TUnitOfWork>(
                optionsAction,
                contextLifetime,
                optionsLifetime);

            return services;
        }
    }
}
