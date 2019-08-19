﻿using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;

namespace UnMango.Extensions.Json.Metadata
{
    /// <summary>
    /// Describes json metadata for a <see cref="Type"/>.
    /// </summary>
    public class JsonMetadata
    {
        /// <summary>
        /// Initializes a new instance of a <see cref="JsonMetadata"/> to describe type <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> this metadata will describe.</param>
        internal JsonMetadata([NotNull] Type type)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            PropertyMappings = new Dictionary<PropertyInfo, string>();
        }

        /// <summary>
        /// Gets the type that this <see cref="JsonMetadata"/> describes.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets the property name mappings for <see cref="Type"/>.
        /// </summary>
        public virtual IDictionary<PropertyInfo, string> PropertyMappings { get; }
    }
}
