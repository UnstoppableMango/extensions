using System;
using JetBrains.Annotations;
using UnMango.Extensions.Json.Metadata.Builders;

namespace UnMango.Extensions.Json.Metadata
{
    /// <summary>
    /// Extensions for interacting with <see cref="JsonPropertyBuilder{T}"/>.
    /// </summary>
    public static class JsonPropertyBuilderExtensions
    {
        /// <summary>
        /// Applies a mapping from the current property to the json name <paramref name="name"/>.
        /// </summary>
        /// <param name="builder">The builder to apply the mapping to.</param>
        /// <param name="name">The json name of the current property.</param>
        /// <returns>The same property builder so calls can be chained.</returns>
        /// <remarks>
        /// The last call to this method takes priority.
        /// </remarks>
        public static JsonPropertyBuilder<T> HasName<T>(
            [NotNull] this JsonPropertyBuilder<T> builder,
            [NotNull] string name)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            return builder.Configure(options => options.PropertyMappings[builder.Property] = name);
        }
    }
}
