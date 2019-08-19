using System.Text.Json;
using UnMango.Extensions.Json.Metadata;

namespace UnMango.Extensions.Json.System.Text
{
    public class PropertyNamingPolicy : JsonNamingPolicy
    {
        private readonly JsonMetadata _options;

        public PropertyNamingPolicy(JsonMetadata options)
        {
            _options = options;
        }

        public override string ConvertName(string name)
        {
            return _options.GetPropertyName(name) ?? name;
        }
    }
}
