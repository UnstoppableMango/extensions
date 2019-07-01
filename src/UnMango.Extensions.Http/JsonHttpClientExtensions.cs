using System;
using System.IO;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace UnMango.Extensions.Http
{
    public static class JsonHttpClientExtensions
    {
        /// <summary>
        /// Serializes the specified value as json into an <see cref="HttpMethod.Post"/> request and sends the request.
        /// </summary>
        /// <typeparam name="T">The type to be serialized as json.</typeparam>
        /// <param name="client">The <see cref="HttpClient"/> to use to send the request.</param>
        /// <param name="requestUri">The uri to send the request to.</param>
        /// <param name="value">The value to be serialized as json.</param>
        /// <param name="cancellationToken">The cancellation token for the asynchronous operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(
            this HttpClient client,
            string requestUri,
            T value,
            CancellationToken cancellationToken = default)
        {
            return PostAsJsonAsync(client, requestUri, value, default, cancellationToken);
        }

        /// <summary>
        /// Serializes the specified value as json into an <see cref="HttpMethod.Post"/> request and sends the request.
        /// </summary>
        /// <typeparam name="T">The type to be serialized as json.</typeparam>
        /// <param name="client">The <see cref="HttpClient"/> to use to send the request.</param>
        /// <param name="requestUri">The uri to send the request to.</param>
        /// <param name="value">The value to be serialized as json.</param>
        /// <param name="serializerOptions">The options to use for json serialization.</param>
        /// <param name="cancellationToken">The cancellation token for the asynchronous operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(
            this HttpClient client,
            string requestUri,
            T value,
            JsonSerializerOptions serializerOptions = default,
            CancellationToken cancellationToken = default)
        {
            if (requestUri == null) throw new ArgumentNullException(nameof(requestUri));

            using var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

            return SendAsJsonAsync(client, request, value, serializerOptions, cancellationToken);
        }

        /// <summary>
        /// Serializes the specified value as json into an <see cref="HttpMethod.Post"/> request and sends the request.
        /// </summary>
        /// <typeparam name="T">The type to be serialized as json.</typeparam>
        /// <param name="client">The <see cref="HttpClient"/> to use to send the request.</param>
        /// <param name="requestUri">The uri to send the request to.</param>
        /// <param name="value">The value to be serialized as json.</param>
        /// <param name="cancellationToken">The cancellation token for the asynchronous operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(
            this HttpClient client,
            Uri requestUri,
            T value,
            CancellationToken cancellationToken = default)
        {
            return PostAsJsonAsync(client, requestUri, value, default, cancellationToken);
        }

        /// <summary>
        /// Serializes the specified value as json into an <see cref="HttpMethod.Post"/> request and sends the request.
        /// </summary>
        /// <typeparam name="T">The type to be serialized as json.</typeparam>
        /// <param name="client">The <see cref="HttpClient"/> to use to send the request.</param>
        /// <param name="requestUri">The uri to send the request to.</param>
        /// <param name="value">The value to be serialized as json.</param>
        /// <param name="serializerOptions">The options to use for json serialization.</param>
        /// <param name="cancellationToken">The cancellation token for the asynchronous operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(
            this HttpClient client,
            Uri requestUri,
            T value,
            JsonSerializerOptions serializerOptions = default,
            CancellationToken cancellationToken = default)
        {
            if (requestUri == null) throw new ArgumentNullException(nameof(requestUri));

            using var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

            return SendAsJsonAsync(client, request, value, serializerOptions, cancellationToken);
        }

        /// <summary>
        /// Serializes the specified value as json into an <see cref="HttpMethod.Put"/> request and sends the request.
        /// </summary>
        /// <typeparam name="T">The type to be serialized as json.</typeparam>
        /// <param name="client">The <see cref="HttpClient"/> to use to send the request.</param>
        /// <param name="requestUri">The uri to send the request to.</param>
        /// <param name="value">The value to be serialized as json.</param>
        /// <param name="cancellationToken">The cancellation token for the asynchronous operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PutAsJsonAsync<T>(
            this HttpClient client,
            string requestUri,
            T value,
            CancellationToken cancellationToken = default)
        {
            return PutAsJsonAsync(client, requestUri, value, default, cancellationToken);
        }

        /// <summary>
        /// Serializes the specified value as json into an <see cref="HttpMethod.Put"/> request and sends the request.
        /// </summary>
        /// <typeparam name="T">The type to be serialized as json.</typeparam>
        /// <param name="client">The <see cref="HttpClient"/> to use to send the request.</param>
        /// <param name="requestUri">The uri to send the request to.</param>
        /// <param name="value">The value to be serialized as json.</param>
        /// <param name="serializerOptions">The options to use for json serialization.</param>
        /// <param name="cancellationToken">The cancellation token for the asynchronous operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PutAsJsonAsync<T>(
            this HttpClient client,
            string requestUri,
            T value,
            JsonSerializerOptions serializerOptions = default,
            CancellationToken cancellationToken = default)
        {
            if (requestUri == null) throw new ArgumentNullException(nameof(requestUri));

            using var request = new HttpRequestMessage(HttpMethod.Put, requestUri);

            return SendAsJsonAsync(client, request, value, serializerOptions, cancellationToken);
        }

        /// <summary>
        /// Serializes the specified value as json into an <see cref="HttpMethod.Put"/> request and sends the request.
        /// </summary>
        /// <typeparam name="T">The type to be serialized as json.</typeparam>
        /// <param name="client">The <see cref="HttpClient"/> to use to send the request.</param>
        /// <param name="requestUri">The uri to send the request to.</param>
        /// <param name="value">The value to be serialized as json.</param>
        /// <param name="cancellationToken">The cancellation token for the asynchronous operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PutAsJsonAsync<T>(
            this HttpClient client,
            Uri requestUri,
            T value,
            CancellationToken cancellationToken = default)
        {
            return PutAsJsonAsync(client, requestUri, value, default, cancellationToken);
        }

        /// <summary>
        /// Serializes the specified value as json into an <see cref="HttpMethod.Put"/> request and sends the request.
        /// </summary>
        /// <typeparam name="T">The type to be serialized as json.</typeparam>
        /// <param name="client">The <see cref="HttpClient"/> to use to send the request.</param>
        /// <param name="requestUri">The uri to send the request to.</param>
        /// <param name="value">The value to be serialized as json.</param>
        /// <param name="serializerOptions">The options to use for json serialization.</param>
        /// <param name="cancellationToken">The cancellation token for the asynchronous operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PutAsJsonAsync<T>(
            this HttpClient client,
            Uri requestUri,
            T value,
            JsonSerializerOptions serializerOptions = default,
            CancellationToken cancellationToken = default)
        {
            if (requestUri == null) throw new ArgumentNullException(nameof(requestUri));

            using var request = new HttpRequestMessage(HttpMethod.Put, requestUri);

            return SendAsJsonAsync(client, request, value, serializerOptions, cancellationToken);
        }

        /// <summary>
        /// Serializes the specified value as json into the content of <paramref name="request"/> and sends the request.
        /// </summary>
        /// <typeparam name="T">The type to be serialized as json.</typeparam>
        /// <param name="client">The <see cref="HttpClient"/> to use to send the request.</param>
        /// <param name="request">The <see cref="HttpRequestMessage"/> to serialize content into and send.</param>
        /// <param name="value">The value to be serialized as json.</param>
        /// <param name="cancellationToken">The cancellation token for the asynchronous operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> SendAsJsonAsync<T>(
            this HttpClient client,
            HttpRequestMessage request,
            T value,
            CancellationToken cancellationToken = default)
        {
            return SendAsJsonAsync(client, request, value, default, cancellationToken);
        }

        /// <summary>
        /// Serializes the specified value as json into the content of <paramref name="request"/> and sends the request.
        /// </summary>
        /// <typeparam name="T">The type to be serialized as json.</typeparam>
        /// <param name="client">The <see cref="HttpClient"/> to use to send the request.</param>
        /// <param name="request">The <see cref="HttpRequestMessage"/> to serialize content into and send.</param>
        /// <param name="value">The value to be serialized as json.</param>
        /// <param name="serializerOptions">The options to use for json serialization.</param>
        /// <param name="cancellationToken">The cancellation token for the asynchronous operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task<HttpResponseMessage> SendAsJsonAsync<T>(
            this HttpClient client,
            HttpRequestMessage request,
            T value,
            JsonSerializerOptions serializerOptions = default,
            CancellationToken cancellationToken = default)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (value == null) throw new ArgumentNullException(nameof(value));

            using var stream = new MemoryStream();
            request.Content = new StreamContent(stream);

            await JsonSerializer.WriteAsync(value, stream, serializerOptions, cancellationToken).ConfigureAwait(false);

            return await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
