using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SandTigerShark.GameServer.Tests.Integrations
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PostAsync<T>(this HttpClient httpClient, string requestUri, T obj)
        {
            var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            return await httpClient.PostAsync(requestUri, content);
        }

        public static async Task<T> ReadAsync<T>(this HttpContent content)
        {
            var json = await content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(json);
        }

        public static async Task<byte[]> ReadByteArrayAsync(this HttpContent content)
        {
            return await content.ReadAsByteArrayAsync();
        }
    }
}