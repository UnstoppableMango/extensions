using System;
using System.Linq.Expressions;
using UnMango.Extensions.Json.Internal;

namespace UnMango.Extensions.Json.Metadata.Builders
{
    /// <summary>
    /// Used to configure <typeparamref name="T"/> for json properties.
    /// </summary>
    /// <typeparam name="T">The type being configured.</typeparam>
    public sealed class JsonTypeBuilder<T> : JsonTypeBuilder
        where T : class
    {
        /// <summary>
        /// Starts a new type property configuration.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being configured.</typeparam>
        /// <param name="selector">Selects the property to configure.</param>
        /// <returns>A property builder for <typeparamref name="T"/>'s property <typeparamref name="TProp"/>.</returns>
        public JsonPropertyBuilder<TProp> Property<TProp>(Expression<Func<T, TProp>> selector)
        {
            var property = selector.GetPropertyAccess();

            return new JsonPropertyBuilder<TProp>(this, property);
        }
    }
}
