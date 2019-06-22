using System.Reflection;

namespace UnMango.Extensions.Json.Metadata.Builders
{
    public class JsonPropertyBuilder<T>
    {
        public JsonPropertyBuilder(JsonTypeBuilder builder, PropertyInfo property)
        {
            TypeBuilder = builder;
            Property = property;
        }

        public JsonTypeBuilder TypeBuilder { get; }

        public PropertyInfo Property { get; }
    }
}
