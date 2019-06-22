using System.Collections.Generic;
using System.Reflection;

namespace UnMango.Extensions.Json.Metadata
{
    public class JsonConfiguration
    {
        public IDictionary<PropertyInfo, string> PropertyMappings { get; } =
            new Dictionary<PropertyInfo, string>();
    }
}
