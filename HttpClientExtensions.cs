using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RestAPIClient
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, HttpContent content)
        {
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri);

            request.Content = content;

            HttpResponseMessage response = await client.SendAsync(request);

            return response;
        }
    }
}