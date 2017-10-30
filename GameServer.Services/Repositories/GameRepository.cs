using SandTigerShark.Services.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameServer.Services.Repositories
{
    internal class GameRepository : IGameRepository
    {
        private ConcurrentDictionary<Guid, GameStatus> games = new ConcurrentDictionary<Guid, GameStatus>();

        public Task<Guid> GetAvailableGame()
        {
            if (games.Count == 0)
            {
                return Task.FromResult(Guid.Empty);
            }
            return Task.FromResult(games.ElementAt(0).Key);
        }

        public Task CreateGame()
        {
            Guid gameToken = Guid.NewGuid();
            games.AddOrUpdate(gameToken, new GameStatus(gameToken.ToString(), true, GameStatus.Status.IN_PROGRESS, null), (token, gameStatusOld) => gameStatusOld);
            return Task.FromResult(gameToken);
        }

        public Task<GameStatus> GetGameStatus(Guid gameId, Guid userToken)
        {
            GameStatus gameStatus = null;

            if (!games.TryGetValue(gameId, out gameStatus))
            {
                throw new NotFoundException();
            }
            return Task.FromResult(gameStatus);
        }

        public Task Clear()
        {
            games.Clear();
            return Task.FromResult(true);
        }
    }
}
