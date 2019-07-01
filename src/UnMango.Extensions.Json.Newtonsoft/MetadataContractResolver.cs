using Newtonsoft.Json.Serialization;
using UnMango.Extensions.Json.Metadata;

namespace UnMango.Extensions.Json.Newtonsoft
{
    public class MetadataContractResolver : DefaultContractResolver
    {
        private readonly JsonMetadata _options;

        public MetadataContractResolver(JsonMetadata options)
        {
            _options = options;
        }

        protected override string ResolvePropertyName(string propertyName)
        {
            return _options.GetPropertyName(propertyName) ?? base.ResolvePropertyName(propertyName);
        }
    }
}
