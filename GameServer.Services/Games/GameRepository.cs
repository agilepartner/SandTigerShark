using SandTigerShark.GameServer.Services.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace SandTigerShark.GameServer.Services.Games
{
    internal class GameRepository : IGameRepository
    {
        private ConcurrentDictionary<Guid, Game> games = new ConcurrentDictionary<Guid, Game>();

        public Task<Guid> GetAvailableGame()
        {
            var availableGame = games.Select(g => g.Value)
                                     .FirstOrDefault(g => g.IsAvailable());

            if(availableGame == null)
            {
                throw new NotFoundException();
            }
            return Task.FromResult(availableGame.Id);
        }

        public Task Save(Game game)
        {
            games.AddOrUpdate(game.Id, game, (id, g) => g);
            return Task.FromResult(true);
        }

        public Task<Game> GetById(Guid gameId)
        {
            Game gameStatus = null;

            if(!games.TryGetValue(gameId, out gameStatus))
            {
                throw new NotFoundException($"No game found with id : {gameId}");
            }
            return Task.FromResult(gameStatus);
        }
    }
}
