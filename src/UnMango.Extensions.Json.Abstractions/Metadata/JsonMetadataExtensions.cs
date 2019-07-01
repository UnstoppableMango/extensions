using System;
using System.Linq;
using System.Reflection;

namespace UnMango.Extensions.Json.Metadata
{
    public static class JsonMetadataExtensions
    {
        public static string GetPropertyName(
            this JsonMetadata options,
            string propertyName,
            StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            return options.PropertyMappings.FirstOrDefault(x => match(x.Key)).Value;

            bool match(PropertyInfo property) => property.Name.Equals(propertyName, stringComparison);
        }
    }
}
