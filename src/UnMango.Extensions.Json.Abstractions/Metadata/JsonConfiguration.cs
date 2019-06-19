using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnMango.Extensions.Json.Metadata
{
    public class JsonConfiguration
    {
        private readonly Dictionary<PropertyInfo, string> _propertyMappings =
            new Dictionary<PropertyInfo, string>();

        public IReadOnlyDictionary<PropertyInfo, string> PropertyMappings => _propertyMappings;

        internal void AddPropertyMapping(PropertyInfo property, string name)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            _propertyMappings.Add(property, name);
        }
    }
}
