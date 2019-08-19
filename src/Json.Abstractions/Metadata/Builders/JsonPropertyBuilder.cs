using System;
using System.Reflection;

namespace UnMango.Extensions.Json.Metadata.Builders
{
    /// <summary>
    /// Allows for adding metadata to the property of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the property being configured.</typeparam>
    public class JsonPropertyBuilder<T>
    {
        /// <summary>
        /// Initializes a new instance of a <see cref="JsonPropertyBuilder{T}"/>.
        /// </summary>
        /// <param name="builder">The base type builder that configurations will be applied to.</param>
        /// <param name="property">The property being configured.</param>
        public JsonPropertyBuilder(JsonTypeBuilder builder, PropertyInfo property)
        {
            if (property.PropertyType != typeof(T))
            {
                throw new ArgumentException($"The type of {property} is not {typeof(T)}", nameof(property));
            }

            TypeBuilder = builder;
            Property = property;
        }

        /// <summary>
        /// Gets the base type builder that configurations will be applied to.
        /// </summary>
        public JsonTypeBuilder TypeBuilder { get; }

        /// <summary>
        /// Gets the property being configured.
        /// </summary>
        public PropertyInfo Property { get; }

        /// <summary>
        /// Adds an action used to configure the <see cref="JsonMetadata"/> for this builder.
        /// </summary>
        /// <param name="configure">The action used to configure the <see cref="JsonMetadata"/> for this builder.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public JsonPropertyBuilder<T> Configure(Action<JsonMetadata> configure)
        {
            if (configure == null) throw new ArgumentNullException(nameof(configure));

            TypeBuilder.Configure(configure);

            return this;
        }
    }
}
