using Newtonsoft.Json;
using SandTigerShark.GameServer.Services.Commands;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SandTigerShark.GameServer.Tests.Integrations
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PostAsync<T>(this HttpClient httpClient, string requestUri, T obj)
        {
            return await httpClient.PostAsJsonAsync(requestUri, obj);
        }
        public static async Task<HttpResponseMessage> PutAsync<T>(this HttpClient httpClient, string requestUri, T obj)
        {
            return await httpClient.PutAsJsonAsync(requestUri, obj);
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

        public static async Task<HttpClient> WithUserToken(
            this HttpClient httpClient,
            string userName)
        {
            var command = new CreateUser { UserName = userName };
            using (var response = await httpClient.PostAsync($"api/users", command))
            {
                var userToken = await response.Content.ReadAsync<Guid>();
                httpClient.DefaultRequestHeaders.Add("user-token", userToken.ToString());

                return httpClient;
            }
        }
    }
}