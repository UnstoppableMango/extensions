using System;
using System.Linq;
using System.Reflection;

namespace UnMango.Extensions.Json.Metadata
{
    /// <summary>
    /// Extension methods for <see cref="JsonMetadata"/>.
    /// </summary>
    public static class JsonMetadataExtensions
    {
        /// <summary>
        /// Gets the name mapped to property <paramref name="propertyName"/>. <see langword="null"/> if no mapping exists.
        /// </summary>
        /// <param name="metadata">The metadata to use as a source of property mappings.</param>
        /// <param name="propertyName">The name of the property for which a mapping should be retrieved.</param>
        /// <param name="stringComparison">The <see cref="StringComparison"/> to use to retrieve the mapping.</param>
        /// <returns>The mapped property name. <see langword="null"/> if no mapping exists.</returns>
        public static string? GetPropertyName(
            this JsonMetadata metadata,
            string propertyName,
            StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            return metadata.PropertyMappings.FirstOrDefault(x => match(x.Key)).Value;

            bool match(PropertyInfo property) => property.Name.Equals(propertyName, stringComparison);
        }
    }
}
