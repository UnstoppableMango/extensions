using System;
using KG.DCX.Extensions.Cqrs;
// ReSharper disable MemberCanBePrivate.Global

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for adding CQRS types to an <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds core CQRS types to <paramref name="services"/>.
        /// </summary>
        /// <param name="services">The service collection to add types to.</param>
        /// <returns>A builder for further configuring how CQRS types are added.</returns>
        public static CqrsBuilder AddCqrs(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // No core types to add in this version

            return new CqrsBuilder(services);
        }

        /// <summary>
        /// Adds core CQRS types to <paramref name="services"/> using the configuration action
        /// specified in <paramref name="configure"/>.
        /// </summary>
        /// <param name="services">The service collection to add types to.</param>
        /// <param name="configure">The action used to configure how services are added.</param>
        /// <returns>The service collection so calls can be chained.</returns>
        public static IServiceCollection AddCqrs(this IServiceCollection services, Action<CqrsBuilder> configure)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configure == null) throw new ArgumentNullException(nameof(configure));

            configure(services.AddCqrs());

            return services;
        }
    }
}
