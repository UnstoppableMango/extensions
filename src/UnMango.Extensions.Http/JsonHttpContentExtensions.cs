using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace UnMango.Extensions.Http
{
    public static class JsonHttpContentExtensions
    {
        public static async Task<T> ReadAsJsonAsync<T>(
            this HttpContent content,
            JsonSerializerOptions options = default,
            CancellationToken cancellationToken = default)
        {
            using var stream = await content.ReadAsStreamAsync().ConfigureAwait(false);

            return await JsonSerializer.ReadAsync<T>(stream, options, cancellationToken).ConfigureAwait(false);
        }
    }
}
