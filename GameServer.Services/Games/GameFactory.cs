using Microsoft.Extensions.Options;
using SandTigerShark.GameServer.Services.Configurations;
using SandTigerShark.GameServer.Services.Http;
using SandTigerShark.GameServer.Services.TicTacToes;
using System;
using System.Collections.Generic;

namespace SandTigerShark.GameServer.Services.Games
{
    internal sealed class GameFactory : IGameFactory
    {
        private readonly IRestProxy restProxy;
        private readonly AzureConfig configuration;
        private readonly IReadOnlyDictionary<GameType, Func<IRestProxy, AzureConfig, Guid, Game>> factory = new Dictionary<GameType, Func<IRestProxy, AzureConfig, Guid, Game>>
        {
            { GameType.TicTacToe, (restProxy, azureConfig, userToken) => new TicTacToe(restProxy, azureConfig, userToken) }
        };

        public GameFactory(
            IRestProxy restProxy,
            IOptions<AzureConfig> configuration)
        {
            this.restProxy = restProxy;
            this.configuration = configuration.Value;
        }

        public Game Create(GameType type, Guid userToken)
        {
            return factory[type](restProxy, configuration, userToken);
        }
    }
}
