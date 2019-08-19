using System;
using Microsoft.Extensions.DependencyInjection;

namespace KG.DCX.Extensions.Cqrs
{
    internal static class CqrsBuilderExtensions
    {
        public delegate Type ServiceTypeFactory(Type type);

        public static CqrsBuilder AddHandlerCore(
            this CqrsBuilder builder,
            ServiceTypeFactory factory,
            Type handlerType,
            ServiceLifetime handlerLifetime)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (handlerType == null) throw new ArgumentNullException(nameof(handlerType));

            var serviceType = factory(handlerType);

            builder.Services.Add(ServiceDescriptor.Describe(
                serviceType,
                handlerType,
                handlerLifetime));

            return builder;
        }
    }
}
