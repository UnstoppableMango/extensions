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
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(
            this HttpClient client,
            string requestUri,
            T value,
            CancellationToken cancellationToken = default)
        {
            return PostAsJsonAsync(client, requestUri, value, default, cancellationToken);
        }

        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(
            this HttpClient client,
            string requestUri,
            T value,
            JsonSerializerOptions options = default,
            CancellationToken cancellationToken = default)
        {
            if (requestUri == null) throw new ArgumentNullException(nameof(requestUri));

            using var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

            return SendAsJsonAsync(client, request, value, options, cancellationToken);
        }

        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(
            this HttpClient client,
            Uri requestUri,
            T value,
            CancellationToken cancellationToken = default)
        {
            return PostAsJsonAsync(client, requestUri, value, default, cancellationToken);
        }

        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(
            this HttpClient client,
            Uri requestUri,
            T value,
            JsonSerializerOptions options = default,
            CancellationToken cancellationToken = default)
        {
            if (requestUri == null) throw new ArgumentNullException(nameof(requestUri));

            using var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

            return SendAsJsonAsync(client, request, value, options, cancellationToken);
        }

        public static Task<HttpResponseMessage> PutAsJsonAsync<T>(
            this HttpClient client,
            string requestUri,
            T value,
            CancellationToken cancellationToken = default)
        {
            return PutAsJsonAsync(client, requestUri, value, default, cancellationToken);
        }

        public static Task<HttpResponseMessage> PutAsJsonAsync<T>(
            this HttpClient client,
            string requestUri,
            T value,
            JsonSerializerOptions options = default,
            CancellationToken cancellationToken = default)
        {
            if (requestUri == null) throw new ArgumentNullException(nameof(requestUri));

            using var request = new HttpRequestMessage(HttpMethod.Put, requestUri);

            return SendAsJsonAsync(client, request, value, options, cancellationToken);
        }

        public static Task<HttpResponseMessage> PutAsJsonAsync<T>(
            this HttpClient client,
            Uri requestUri,
            T value,
            CancellationToken cancellationToken = default)
        {
            return PutAsJsonAsync(client, requestUri, value, default, cancellationToken);
        }

        public static Task<HttpResponseMessage> PutAsJsonAsync<T>(
            this HttpClient client,
            Uri requestUri,
            T value,
            JsonSerializerOptions options = default,
            CancellationToken cancellationToken = default)
        {
            if (requestUri == null) throw new ArgumentNullException(nameof(requestUri));

            using var request = new HttpRequestMessage(HttpMethod.Put, requestUri);

            return SendAsJsonAsync(client, request, value, options, cancellationToken);
        }

        public static Task<HttpResponseMessage> SendAsJsonAsync<T>(
            this HttpClient client,
            HttpRequestMessage request,
            T value,
            CancellationToken cancellationToken = default)
        {
            return SendAsJsonAsync(client, request, value, default, cancellationToken);
        }

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
