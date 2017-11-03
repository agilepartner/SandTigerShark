using Microsoft.Extensions.Options;
using SandTigerShark.TicTacToe.App.Commands;
using System;
using System.Threading.Tasks;

namespace SandTigerShark.TicTacToe.App
{
    internal sealed class UserRepository
    {
        private readonly IRestProxy restProxy;
        private readonly string baseUrl;

        public UserRepository(
            IOptions<GameConfig> configuration,
            IRestProxy proxy)
        {
            restProxy = proxy;
            baseUrl = $"{configuration.Value.Url}users";
        }

        public async Task<Guid> Create(string userName)
        {
            var command = new CreateUser { UserName = userName };
            return await restProxy.PostAsync<CreateUser, Guid>(baseUrl, command);
        }
    }
}
