using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SandTigerShark.GameServer.Services.Http
{
    public interface IRestProxy
    {
        Task<TResult> GetAsync<TResult>(string requestUri, CancellationToken? cancellationToken = null);
        Task<string> GetStringAsync(string requestUri, CancellationToken? cancellationToken = null);
        Task<byte[]> GetByteArrayAsync(string requestUri, CancellationToken? cancellationToken = null);

        Task<TResult> PostAsync<TBody, TResult>(string requestUri, TBody body, CancellationToken? cancellationToken = null);
        Task<string> PostAsync<TBody>(string requestUri, TBody body, CancellationToken? cancellationToken = null);
        Task<string> PostFormUrlEncodedAsync(string requestUri, IEnumerable<KeyValuePair<string, string>> body, CancellationToken? cancellationToken = null);
    }
}