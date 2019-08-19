using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace UnMango.Extensions.Http
{
    /// <summary>
    /// Extension methods for interacting with json HTTP content
    /// </summary>
    public static class JsonHttpContentExtensions
    {
        /// <summary>
        /// Reads <paramref name="content"/> as json into a new <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to deserialize as.</typeparam>
        /// <param name="content">The HTTP content to read from.</param>
        /// <param name="cancellationToken">The cancellation token for the asynchronous operation.</param>
        /// <returns>The <see cref="HttpContent"/> deserialized as a <typeparamref name="T"/>.</returns>
        public static Task<T> ReadAsJsonAsync<T>(
            this HttpContent content,
            CancellationToken cancellationToken = default)
        {
            return ReadAsJsonAsync<T>(content, default, cancellationToken);
        }

        /// <summary>
        /// Reads <paramref name="content"/> as json into a new <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to deserialize as.</typeparam>
        /// <param name="content">The HTTP content to read from.</param>
        /// <param name="options">The serializer options for the operation.</param>
        /// <param name="cancellationToken">The cancellation token for the asynchronous operation.</param>
        /// <returns>The <see cref="HttpContent"/> deserialized as a <typeparamref name="T"/>.</returns>
        public static async Task<T> ReadAsJsonAsync<T>(
            [NotNull] this HttpContent content,
            JsonSerializerOptions? options = default,
            CancellationToken cancellationToken = default)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            using var stream = await content.ReadAsStreamAsync().ConfigureAwait(false);

            return await JsonSerializer.DeserializeAsync<T>(stream, options, cancellationToken).ConfigureAwait(false);
        }
    }
}
