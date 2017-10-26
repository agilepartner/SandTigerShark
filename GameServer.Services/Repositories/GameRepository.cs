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
        // private static ConcurrentDictionary<string, GameStatus> games = new ConcurrentDictionary<string, GameStatus>();
        private List<Guid> games = new List<Guid>();

        Guid IGameRepository.GetAvailableGame()
        {
            if (this.games.Count == 0)
            {
                return Guid.Empty;
            }

            return this.games.ElementAt(0);
        }

        void IGameRepository.CreateGame()
        {
            this.games.Add(Guid.NewGuid());
        }

        /*
         * Rollback
        public Task<GameStatus> GetGameStatus(string gameId)
        {
            var gameStatus = games.SingleOrDefault(entry => entry.Value.GetId().Equals(gameId)).Value;

            if (gameStatus == null)
            {
                throw new NotFoundException();
            }

            return Task.FromResult(gameStatus);
        }
        */

        // public string GetOrCreateNewGame(string userToken)
        // {
        //      return statusesFromUserId.GetOrAdd(
        // userToken,
        //  new GameStatus(Guid.NewGuid().ToString(), true, GameStatus.Status.IN_PROGRESS, null)).GetId();
        //  }


        Task<GameStatus> IGameRepository.GetGameStatus(string gameId)
        {
            throw new NotImplementedException();
        }
    }
}
