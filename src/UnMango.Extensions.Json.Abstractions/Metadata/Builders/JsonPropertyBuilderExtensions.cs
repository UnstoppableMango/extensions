using System;

namespace UnMango.Extensions.Json.Metadata.Builders
{
    internal static class JsonPropertyBuilderExtensions
    {
        public static JsonPropertyBuilder<T> Configure<T>(
            this JsonPropertyBuilder<T> builder,
            Action<JsonConfiguration> configure)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configure == null) throw new ArgumentNullException(nameof(configure));

            builder.Builder.Configure(configure);

            return builder;
        }
    }
}
