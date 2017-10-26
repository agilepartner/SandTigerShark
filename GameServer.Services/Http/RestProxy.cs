using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SandTigerShark.GameServer.Services.Http
{
    public sealed class RestProxy : IRestProxy
    {
        private readonly HttpClientConfig _configuration;

        public RestProxy(IOptions<HttpClientConfig> configuration)
        {
            _configuration = configuration.Value;
        }

        #region Get

        public async Task<TResult> GetAsync<TResult>(string requestUri, CancellationToken? cancellationToken = null)
        {
            return await GetAsync(requestUri, async(c) => await ReadAsync<TResult>(c), cancellationToken);
        }

        public async Task<string> GetStringAsync(string requestUri, CancellationToken? cancellationToken = null)
        {
            return await GetAsync(requestUri, (c) => c.ReadAsStringAsync(), cancellationToken);
        }

        public async Task<byte[]> GetByteArrayAsync(string requestUri, CancellationToken? cancellationToken = null)
        {
            return await GetAsync(requestUri, (c) => c.ReadAsByteArrayAsync(), cancellationToken);
        }

        private async Task<TResult> GetAsync<TResult>(string requestUri, Func<HttpContent, Task<TResult>> readMethod, CancellationToken? cancellationToken = null)
        {
            using (HttpClient client = CreateHttpClient())
            {
                var response = await (cancellationToken.HasValue ? client.GetAsync(requestUri, cancellationToken.Value) : client.GetAsync(requestUri));
                CheckSuccessCode(response);

                return await readMethod(response.Content);
            }
        }

        private async Task<T> ReadAsync<T>(HttpContent content)
        {
            var json = await content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(json);
        }

        #endregion

        #region Post

        public async Task<TResult> PostAsync<TBody, TResult>(string requestUri, TBody body, CancellationToken? cancellationToken = null)
        {
            return await PostAsync(requestUri, body, async (c) => await ReadAsync<TResult>(c), cancellationToken);
        }

        public async Task<string> PostAsync<TBody>(string requestUri, TBody body, CancellationToken? cancellationToken = null)
        {
            return await PostAsync(requestUri, body, httpContent => httpContent.ReadAsStringAsync(), cancellationToken);
        }

        public async Task<string> PostFormUrlEncodedAsync(string requestUri, IEnumerable<KeyValuePair<string, string>> body, CancellationToken? cancellationToken = null)
        {
            var content = new FormUrlEncodedContent(body);
            return await PostContentAsync(requestUri, content, async(httpContent) => await httpContent.ReadAsStringAsync(), cancellationToken);
        }

        private async Task<TResult> PostAsync<TBody, TResult>(string requestUri, TBody body, Func<HttpContent, Task<TResult>> readMethod, CancellationToken? cancellationToken = null)
        {
            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, _configuration.DefaultContentType);
            return await PostContentAsync(requestUri, content, readMethod, cancellationToken);
        }

        private async Task<TResult> PostContentAsync<TResult>(string requestUri, HttpContent content, Func<HttpContent, Task<TResult>> readMethod, CancellationToken? cancellationToken = null)
        {
            using (HttpClient client = CreateHttpClient())
            {
                var response = await (cancellationToken.HasValue ? client.PostAsync(requestUri, content, cancellationToken.Value) : client.PostAsync(requestUri, content));
                CheckSuccessCode(response);

                return await readMethod(response.Content);
            }
        }

        #endregion

        private HttpClient CreateHttpClient()
        {
            HttpClient client;

            if (string.IsNullOrEmpty(_configuration.ProxyUri))
            {
                client = new HttpClient();
            }
            else
            {
                var handler = new HttpClientHandler
                {
                    UseProxy = !string.IsNullOrEmpty(_configuration.ProxyUri),
                    Proxy = new CustomWebProxy(_configuration.ProxyUri)
                };

                client = new HttpClient(handler);
            }
            
            client.Timeout = TimeSpan.FromSeconds(_configuration.Timeout);            
            return client;
        }

        private void CheckSuccessCode(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"{response.ReasonPhrase} : {response.RequestMessage.RequestUri.AbsoluteUri}");
            }
        }        
    }
}
