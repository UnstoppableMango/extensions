using Microsoft.Extensions.DependencyInjection;

namespace KG.DCX.Extensions.Cqrs
{
    /// <summary>
    /// Allows for configuring how CQRS types are added to an application.
    /// </summary>
    public class CqrsBuilder
    {
        internal const ServiceLifetime DEFAULT_HANDLER_LIFETIME = ServiceLifetime.Scoped;

        internal CqrsBuilder(IServiceCollection services)
        {
            Services = services;
        }

        /// <summary>
        /// Gets the <see cref="IServiceCollection"/> this builder will add to.
        /// </summary>
        public IServiceCollection Services { get; }
    }
}
