using System;
using System.IO;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace UnMango.Extensions.Http
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(
            this HttpClient client,
            string requestUri,
            T value,
            JsonSerializerOptions options = default,
            CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

            return SendAsJsonAsync(client, request, value, options, cancellationToken);
        }

        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(
            this HttpClient client,
            Uri requestUri,
            T value,
            JsonSerializerOptions options = default,
            CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

            return SendAsJsonAsync(client, request, value, options, cancellationToken);
        }

        public static Task<HttpResponseMessage> PutAsJsonAsync<T>(
            this HttpClient client,
            string requestUri,
            T value,
            JsonSerializerOptions options = default,
            CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(HttpMethod.Put, requestUri);

            return SendAsJsonAsync(client, request, value, options, cancellationToken);
        }

        public static Task<HttpResponseMessage> PutAsJsonAsync<T>(
            this HttpClient client,
            Uri requestUri,
            T value,
            JsonSerializerOptions options = default,
            CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(HttpMethod.Put, requestUri);

            return SendAsJsonAsync(client, request, value, options, cancellationToken);
        }

        public static async Task<HttpResponseMessage> SendAsJsonAsync<T>(
            this HttpClient client,
            HttpRequestMessage request,
            T value,
            JsonSerializerOptions serializerOptions,
            CancellationToken cancellationToken = default)
        {
            using var stream = new MemoryStream();
            request.Content = new StreamContent(stream);

            await JsonSerializer.WriteAsync(value, stream, serializerOptions, cancellationToken).ConfigureAwait(false);

            return await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
