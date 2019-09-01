using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using UnMango.Extensions.Repository;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for <see cref="IServiceCollection"/> for registering types in assemblies.
    /// </summary>
    public static class ServiceCollectionAssemblyExtensions
    {
        /// <summary>
        /// Searches loaded assemblies and registers all <see cref="IRepository"/> implementations found.
        /// Will attempt to register an <see cref="IUnitOfWork"/> implementation as well. If more than one is
        /// found, an exception will be thrown.
        /// </summary>
        /// <param name="services">The collection to add registrations to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so calls can be chained.</returns>
        [UsedImplicitly]
        public static IServiceCollection AddRepositoryPattern(this IServiceCollection services)
        {
            var assemblies = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(NotLibraryAssembly);

            return AddRepositoryPattern(services, assemblies);
        }

        /// <summary>
        /// Searches the specified assemblies and registers all <see cref="IRepository"/> implementations found.
        /// Will attempt to register an <see cref="IUnitOfWork"/> implementation as well. If more than one is
        /// found, an exception will be thrown.
        /// </summary>
        /// <param name="services">The collection to add registrations to.</param>
        /// <param name="assemblies">The assemblies to retrieve types from.</param>
        /// <returns>The <see cref="IServiceCollection"/> so calls can be chained.</returns>
        [UsedImplicitly]
        public static IServiceCollection AddRepositoryPattern(
            this IServiceCollection services,
            params Assembly[] assemblies)
            => AddRepositoryPattern(services, assemblies.AsEnumerable());

        /// <summary>
        /// Searches the specified assemblies and registers all <see cref="IRepository"/> implementations found.
        /// Will attempt to register an <see cref="IUnitOfWork"/> implementation as well. If more than one is
        /// found, an exception will be thrown.
        /// </summary>
        /// <param name="services">The collection to add registrations to.</param>
        /// <param name="assemblies">The assemblies to retrieve types from.</param>
        /// <returns>The <see cref="IServiceCollection"/> so calls can be chained.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when multiple implementations of <see cref="IUnitOfWork"/> are found.
        /// </exception>
        [UsedImplicitly]
        public static IServiceCollection AddRepositoryPattern(
            this IServiceCollection services,
            IEnumerable<Assembly> assemblies)
        {
            var arr = assemblies as List<Assembly> ?? assemblies.ToList();

            services.AddRepositories(arr);
            services.AddUnitOfWork(arr, false);

            return services;
        }

        /// <summary>
        /// Registers all repositories from each <see cref="Assembly"/>.
        /// </summary>
        /// <param name="services">The collection to add registrations to.</param>
        /// <param name="assemblies">The assemblies to retrieve types from.</param>
        /// <returns>The <see cref="IServiceCollection"/> so calls can be chained.</returns>
        [UsedImplicitly]
        public static IServiceCollection AddRepositories(this IServiceCollection services, params Assembly[] assemblies)
            => services.AddRepositories(assemblies.AsEnumerable());

        /// <summary>
        /// Registers all repositories from each <see cref="Assembly"/>.
        /// </summary>
        /// <param name="services">The collection to add registrations to.</param>
        /// <param name="assemblies">The assemblies to retrieve types from.</param>
        /// <returns>The <see cref="IServiceCollection"/> so calls can be chained.</returns>
        [UsedImplicitly]
        public static IServiceCollection AddRepositories(
            this IServiceCollection services,
            IEnumerable<Assembly> assemblies)
        {
            var types = assemblies
                .SelectMany(x => x.GetTypes())
                .Where(RepoUtility.ImplementsIRepository);

            foreach (var type in types)
                services.AddRepository(type);

            return services;
        }

        /// <summary>
        /// Registers <typeparamref name="T"/> as an <see cref="IUnitOfWork"/> and registers all
        /// repositories from the given assemblies.
        /// </summary>
        /// <typeparam name="T">The implementation type of <see cref="IUnitOfWork"/>.</typeparam>
        /// <param name="services">The collection to add registrations to.</param>
        /// <param name="assemblies">The assemblies to retrieve types from.</param>
        /// <returns>The <see cref="IServiceCollection"/> so calls can be chained.</returns>
        [UsedImplicitly]
        public static IServiceCollection AddUnitOfWork<T>(
            this IServiceCollection services,
            params Assembly[] assemblies)
            where T : class, IUnitOfWork
            => services.AddUnitOfWork<T>(assemblies.AsEnumerable());

        /// <summary>
        /// Registers <typeparamref name="T"/> as an <see cref="IUnitOfWork"/> and registers all
        /// repositories from the given assemblies.
        /// </summary>
        /// <typeparam name="T">The implementation type of <see cref="IUnitOfWork"/>.</typeparam>
        /// <param name="services">The collection to add registrations to.</param>
        /// <param name="assemblies">The assemblies to retrieve types from.</param>
        /// <returns>The <see cref="IServiceCollection"/> so calls can be chained.</returns>
        [UsedImplicitly]
        public static IServiceCollection AddUnitOfWork<T>(
            this IServiceCollection services,
            IEnumerable<Assembly> assemblies)
            where T : class, IUnitOfWork
        {
            services.AddRepositories(assemblies);

            return services.AddUnitOfWork<T>();
        }

        /// <summary>
        /// Searches for an <see cref="IUnitOfWork"/> in the given assemblies and registers is.
        /// If multiple implementations are found, an exception will be thrown.
        /// </summary>
        /// <param name="services">The collection to add registrations to.</param>
        /// <param name="assemblies">The assemblies to retrieve types from.</param>
        /// <returns>The <see cref="IServiceCollection"/> so calls can be chained.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when multiple implementations of <see cref="IUnitOfWork"/> are found.
        /// </exception>
        [UsedImplicitly]
        public static IServiceCollection AddUnitOfWork(
            this IServiceCollection services,
            IEnumerable<Assembly> assemblies)
            => AddUnitOfWork(services, assemblies, false);

        /// <summary>
        /// <para>
        ///     Searches for an <see cref="IUnitOfWork"/> in the given assemblies. If <paramref name="registerMultiple"/>
        ///     is <see langword="false"/> and more than one implementation is found, it will throw an exception.
        ///     If only one implementation is found, it will be added to <paramref name="services"/>.
        ///     If multiple implementations are found and <paramref name="registerMultiple"/> is <see langword="true"/>,
        ///     all will be registered.
        /// </para>
        /// </summary>
        /// <param name="services">The collection to add registrations to.</param>
        /// <param name="assemblies">The assemblies to retrieve types from.</param>
        /// <param name="registerMultiple">Whether or not to throw when multiple implementations are found.</param>
        /// <returns>The <see cref="IServiceCollection"/> so calls can be chained.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when multiple implementations of <see cref="IUnitOfWork"/> are found and
        /// <paramref name="registerMultiple"/> is set to <see langword="false"/>.
        /// </exception>
        [UsedImplicitly]
        public static IServiceCollection AddUnitOfWork(
            this IServiceCollection services,
            IEnumerable<Assembly> assemblies,
            bool registerMultiple)
        {
            var types = assemblies
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(IUnitOfWork).IsAssignableFrom(x))
                .ToList();

            if (!registerMultiple && types.Count > 1)
            {
                var message = $"More than one implementation of {nameof(IUnitOfWork)} was found";
                throw new InvalidOperationException(message);
            }

            foreach (var type in types)
                services.AddUnitOfWork(type);

            return services;
        }

        private static bool NotLibraryAssembly(Assembly assembly) => !assembly.FullName.Contains(nameof(UnMango.Extensions.Repository));
    }
}
