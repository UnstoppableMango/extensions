using System;
using JetBrains.Annotations;
using KG.DCX.Extensions.Cqrs;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for configuring a <see cref="CqrsBuilder"/>.
    /// </summary>
    public static class CqrsBuilderExtensions
    {
        /// <summary>
        /// Use the default dispatcher implementation with dependencies resolved
        /// using <see cref="IServiceProvider.GetService"/>.
        /// </summary>
        /// <param name="builder">The builder to configure.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static CqrsBuilder UseDispatchers([NotNull] this CqrsBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.AddDispatchers(services => services.GetService);

            return builder;
        }

        /// <summary>
        /// Use the default dispatcher implementation with dependencies resolved
        /// using <paramref name="serviceFactory"/>.
        /// </summary>
        /// <param name="builder">The builder to configure.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static CqrsBuilder UseDispatchers(
            [NotNull] this CqrsBuilder builder,
            [NotNull] Func<Type, object> serviceFactory)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (serviceFactory == null)
            {
                throw new ArgumentNullException(nameof(serviceFactory));
            }

            builder.Services.AddDispatchers(() => serviceFactory.Invoke);

            return builder;
        }

        /// <summary>
        /// Use the default dispatcher implementation with dependencies resolved
        /// using the function returned by <paramref name="serviceFactory"/>.
        /// </summary>
        /// <param name="builder">The builder to configure.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static CqrsBuilder UseDispatchers(
            [NotNull] this CqrsBuilder builder,
            [NotNull] Func<IServiceProvider, Func<Type, object>> serviceFactory)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (serviceFactory == null)
            {
                throw new ArgumentNullException(nameof(serviceFactory));
            }

            builder.Services.AddDispatchers(services => serviceFactory(services).Invoke);

            return builder;
        }
    }
}
