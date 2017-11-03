using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SandTigerShark.TicTacToe.App
{
    public interface IRestProxy
    {
        void AddHttpHeader(string header, string value);
        Task<TResult> GetAsync<TResult>(string requestUri, Func<HttpResponseMessage, Task<TResult>> retryLogic = null);
        Task<TResult> PostAsync<TBody, TResult>(string requestUri, TBody body);
        Task<string> PutAsync<TBody>(string requestUri, TBody body);
    }
}