using Microsoft.Extensions.Options;
using SandTigerShark.GameServer.Services.Commands;
using SandTigerShark.GameServer.Services.Configurations;
using SandTigerShark.GameServer.Services.Http;
using System;
using System.Threading.Tasks;

namespace SandTigerShark.GameServer.Services.TicTacToe
{
    internal class TicTacToeService : ITicTacToeService
    {
        private readonly IRestProxy restProxy;
        private readonly string url;

        public TicTacToeService(
            IRestProxy restProxy,
            IOptions<AzureConfig> configuration)
        {
            this.restProxy = restProxy;
            url = configuration.Value.TicTacToe;
        }

        public async Task Play(Play command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            await restProxy.PostAsync(url, command);
        }
    }
}