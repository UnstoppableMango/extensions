using UnMango.Extensions.Json.Metadata.Builders;

namespace UnMango.Extensions.Json
{
    /// <summary>
    /// Defines an interface for configuring type <typeparamref name="T"/> for json properties.
    /// </summary>
    /// <typeparam name="T">The type to configure.</typeparam>
    public interface IJsonTypeConfiguration<T>
    {
        /// <summary>
        /// Configures a builder for json properties of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="builder">The builder to be configured.</param>
        void Configure(JsonTypeBuilder<T> builder);
    }
}
