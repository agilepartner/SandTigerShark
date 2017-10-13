using SandTigerShark.Models;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace GameServer.Repositories
{
    internal class GameRepository : IGameRepository
    {
        private static ConcurrentDictionary<string, GameStatus> statusesFromUserId = new ConcurrentDictionary<string, GameStatus>();

        public Task<GameStatus> GetGameStatus(string gameId)
        {
            var gameStatus = statusesFromUserId.SingleOrDefault(entry => entry.Value.GetId().Equals(gameId)).Value;

            if(gameStatus == null)
            {
                throw new NotFoundException();
            }

            return Task.FromResult(gameStatus);
        }

        public string GetOrCreateNewGame(string userToken)
        {
            return statusesFromUserId.GetOrAdd(
                       userToken,
                       new GameStatus(Guid.NewGuid().ToString(), true, GameStatus.Status.IN_PROGRESS, null)).GetId();
        }
    }
}
