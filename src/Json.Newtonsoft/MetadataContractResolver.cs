using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Serialization;
using UnMango.Extensions.Json.Metadata;

namespace UnMango.Extensions.Json.Newtonsoft
{
    public class MetadataContractResolver : DefaultContractResolver
    {
        private readonly IDictionary<Type, JsonMetadata> _metadata;
        private Type? _objectType;

        public MetadataContractResolver(IEnumerable<JsonMetadata> metadata)
        {
            _metadata = metadata.ToDictionary(x => x.Type, x => x);
        }

        public string? GetPropertyName(string propertyName)
        {
            if (_objectType == null) return null;

            if (_metadata.TryGetValue(_objectType, out var metadata))
            {
                return metadata.GetPropertyName(propertyName);
            }

            return null;
        }

        protected override JsonContract CreateContract(Type objectType)
        {
            _objectType = objectType;

            return base.CreateContract(objectType);
        }

        protected override string ResolvePropertyName(string propertyName)
        {
            return GetPropertyName(propertyName) ?? base.ResolvePropertyName(propertyName);
        }
    }
}
