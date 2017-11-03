using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Net;

namespace SandTigerShark.TicTacToe.App
{
    public sealed class RestProxy : IRestProxy
    {
        private readonly HttpClientConfig configuration;
        private readonly IDictionary<string, string> additionalHeaders;

        public RestProxy(IOptions<HttpClientConfig> configuration)
        {
            this.configuration = configuration.Value;
            additionalHeaders = new Dictionary<string, string>();
        }

        #region Get

        public async Task<TResult> GetAsync<TResult>(string requestUri, 
            Func<HttpResponseMessage, Task<TResult>> retryLogic = null)
        {
            using (HttpClient client = CreateHttpClient())
            {
                var response = await client.GetAsync(requestUri);

                if (CheckSuccessCode(response, retryLogic == null))
                {
                    return await ReadAsync<TResult>(response.Content);
                }
                else
                {
                    return await retryLogic(response);
                }
            }
        }

        private async Task<T> ReadAsync<T>(HttpContent content)
        {
            var json = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        #endregion

        #region Post

        public async Task<TResult> PostAsync<TBody, TResult>(string requestUri, TBody body)
        {
            return await PostAsync(requestUri, body, async (c) => await ReadAsync<TResult>(c));
        }

        private async Task<TResult> PostAsync<TBody, TResult>(string requestUri, TBody body, Func<HttpContent, Task<TResult>> readMethod)
        {
            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, configuration.DefaultContentType);
            return await PostContentAsync(requestUri, content, readMethod);
        }

        private async Task<TResult> PostContentAsync<TResult>(
            string requestUri, 
            HttpContent content,
            Func<HttpContent, Task<TResult>> readMethod)
        {
            using (HttpClient client = CreateHttpClient())
            {
                var response = await client.PostAsync(requestUri, content);
                CheckSuccessCode(response, true);

                return await readMethod(response.Content);
            }
        }

        #endregion

        private HttpClient CreateHttpClient()
        {
            HttpClient client;

            if (string.IsNullOrEmpty(configuration.ProxyUri))
            {
                client = new HttpClient();
            }
            else
            {
                var handler = new HttpClientHandler
                {
                    UseProxy = !string.IsNullOrEmpty(configuration.ProxyUri),
                    Proxy = new CustomWebProxy(configuration.ProxyUri)
                };

                client = new HttpClient(handler);
            }
            
            client.Timeout = TimeSpan.FromSeconds(configuration.Timeout);
            additionalHeaders.ToList().ForEach(header => client.DefaultRequestHeaders.Add(header.Key, header.Value));

            return client;
        }

        private bool CheckSuccessCode(HttpResponseMessage response, bool throwException)
        {
            if (!response.IsSuccessStatusCode)
            {
                if(throwException)
                {
                    var message = response.Content.ReadAsStringAsync().Result;
                    throw new HttpRequestException($"{response.ReasonPhrase} : {message} at {response.RequestMessage.RequestUri.AbsoluteUri}");
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public void AddHttpHeader(string header, string value)
        {
            additionalHeaders.Add(header, value);
        }

        #region Put


        public async Task<string> PutAsync<TBody>(string requestUri, TBody body)
        {
            return await PutAsync(requestUri, body, async (c) => await ReadAsync<string>(c));
        }

        private async Task<TResult> PutAsync<TBody, TResult>(string requestUri, TBody body, Func<HttpContent, Task<TResult>> readMethod)
        {
            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, configuration.DefaultContentType);
            return await PutContentAsync(requestUri, content, readMethod);
        }

        private async Task<TResult> PutContentAsync<TResult>(
            string requestUri,
            HttpContent content,
            Func<HttpContent, Task<TResult>> readMethod)
        {
            using (HttpClient client = CreateHttpClient())
            {
                var response = await client.PutAsync(requestUri, content);
                CheckSuccessCode(response, true);

                return await readMethod(response.Content);
            }
        }

        #endregion
    }
}
