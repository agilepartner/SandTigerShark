using SandTigerShark.Services.Models;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace GameServer.Services.Repositories
{
    internal class GameRepository : IGameRepository
    {
        private static ConcurrentDictionary<string, GameStatus> statusesFromUserId = new ConcurrentDictionary<string, GameStatus>();

        string IGameRepository.GetAvailableGame()
        {
            throw new NotImplementedException();
        }

        void IGameRepository.CreateGame()
        {
            throw new NotImplementedException();
        }

        public Task<GameStatus> GetGameStatus(string gameId, string userToken)
        {
            var gameStatus = statusesFromUserId.SingleOrDefault(entry => entry.Value.Id.Equals(gameId)).Value;

            if (gameStatus == null)
            {
                throw new NotFoundException();
            }

            return Task.FromResult(gameStatus);
        }
    }
}
