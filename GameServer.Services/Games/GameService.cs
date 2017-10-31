using SandTigerShark.GameServer.Services.Commands;
using SandTigerShark.GameServer.Services.Dtos;
using SandTigerShark.GameServer.Services.Exceptions;
using SandTigerShark.GameServer.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SandTigerShark.GameServer.Services.Games
{
    internal class GameService : IGameService
    {
        private readonly IGameFactory factory;
        private readonly IGameRepository repository;
        private readonly IReadOnlyList<string> availableGameTypes;

        public GameService(
            IGameFactory gameFactory,
            IGameRepository gameRepository)
        {
            this.factory = gameFactory;
            this.repository = gameRepository;
            availableGameTypes = Reflect.GetEnumNames(typeof(GameType)).ToList().AsReadOnly();
        }

        public Task<Guid> CreateGame(CreateGame command, Guid userToken)
        {
            if(command == null)
            {
                throw new InvalidCommandException($"CreateGame command is required");
            }

            if (string.IsNullOrEmpty(command.Type))
            {
                throw new InvalidCommandException($"{nameof(command.Type)} is required in order to create a new game");
            }

            if(!availableGameTypes.Contains(command.Type))
            {
                throw new GameTypeNotAvailableException(command.Type);
            }

            var game = factory.Create(Enum.Parse<GameType>(command.Type), userToken);
            repository.Save(game);

            return Task.FromResult(game.Id);
        }

        public async Task<Guid> GetAvailableGame(GameType gameType, Guid userToken)
        {
            return await repository.GetAvailableGame(gameType);
        }

        public async Task<GameStatus> GetGameStatus(Guid gameId, Guid userToken)
        {
            var game = await repository.GetById(gameId);

            return new GameStatus
            {
                LastState = game.LastGameState,
                Status = game.State
            };
        }

        public async Task Play(Play command, Guid userToken)
        {
            if (command == null)
            {
                throw new InvalidCommandException($"Play command is required");
            }

            if (command.Command == null)
            {
                throw new InvalidCommandException($"{nameof(command.Command)} is required");
            }

            var game = await repository.GetById(command.GameId);

            if(game.IsAvailable())
            {
                throw new InvalidCommandException($"The game is still waiting for player(s)");
            }
            await game.Play(command, userToken);
        }
    }
}