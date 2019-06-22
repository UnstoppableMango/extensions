using System;
using System.Collections.Generic;

namespace UnMango.Extensions.Json.Metadata.Builders
{
    public abstract class JsonTypeBuilder
    {
        private readonly ICollection<Action<JsonConfiguration>> _configures =
            new List<Action<JsonConfiguration>>();

        /// <summary>
        /// Builds the json configuration based on the applied configurations.
        /// </summary>
        /// <returns>The final json configuraion.</returns>
        public JsonConfiguration Build()
        {
            var configuration = new JsonConfiguration();

            foreach (var configure in _configures)
            {
                configure(configuration);
            }

            return configuration;
        }

        public void Configure(Action<JsonConfiguration> configure) => _configures.Add(configure);
    }
}
