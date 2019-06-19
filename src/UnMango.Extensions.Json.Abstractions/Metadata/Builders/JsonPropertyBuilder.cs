using System.Reflection;

namespace UnMango.Extensions.Json.Metadata.Builders
{
    public class JsonPropertyBuilder<T>
    {
        public JsonPropertyBuilder(JsonTypeBuilder builder, PropertyInfo property)
        {
            Builder = builder;
            Property = property;
        }

        internal JsonTypeBuilder Builder { get; }

        internal PropertyInfo Property { get; }
    }
}
