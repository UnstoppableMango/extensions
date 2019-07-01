using System;
using System.Collections.Generic;

namespace UnMango.Extensions.Json.Metadata.Builders
{
    public abstract class JsonTypeBuilder
    {
        private readonly ICollection<Action<JsonMetadata>> _configures =
            new List<Action<JsonMetadata>>();

        /// <summary>
        /// Builds the json configuration based on the applied configurations.
        /// </summary>
        /// <returns>The final json configuraion.</returns>
        public JsonMetadata Build()
        {
            var metadata = NewMetadata();

            foreach (var configure in _configures)
            {
                configure(metadata);
            }

            return metadata;
        }

        /// <summary>
        /// Adds an action used to configure the <see cref="JsonMetadata"/> for this builder.
        /// </summary>
        /// <param name="configure">The action configuring the metadata.</param>
        public void Configure(Action<JsonMetadata> configure) => _configures.Add(configure);

        protected abstract JsonMetadata NewMetadata();
    }
}
