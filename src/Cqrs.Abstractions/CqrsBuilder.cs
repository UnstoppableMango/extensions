using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace KG.DCX.Extensions.Cqrs
{
    /// <summary>
    /// Allows for configuring how CQRS types are added to an application.
    /// </summary>
    public class CqrsBuilder
    {
        internal const ServiceLifetime DEFAULT_HANDLER_LIFETIME = ServiceLifetime.Scoped;

        internal CqrsBuilder([NotNull] IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <summary>
        /// Gets the <see cref="IServiceCollection"/> this builder will add to.
        /// </summary>
        public IServiceCollection Services { get; }
    }
}
