using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace KG.DCX.Extensions.Cqrs
{
    internal static class CqrsBuilderExtensions
    {
        public delegate Type ServiceTypeFactory(Type type);

        public static CqrsBuilder AddHandlerCore(
            [NotNull] this CqrsBuilder builder,
            [NotNull] ServiceTypeFactory factory,
            [NotNull] Type handlerType,
            ServiceLifetime handlerLifetime)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (factory == null) throw new ArgumentNullException(nameof(factory));
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
