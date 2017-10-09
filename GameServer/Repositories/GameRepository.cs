using SandTigerShark.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameServer.Repositories
{
    public class GameRepository
    {

        private static ConcurrentDictionary<String, GameStatus> statusesFromUserId = new ConcurrentDictionary<String, GameStatus>();


        public GameStatus GetGameStatus(String gameId)
        {
            return statusesFromUserId.FirstOrDefault(entry => entry.Value.GetId().Equals(gameId)).Value;
        }

        public string GetOrCreateNewGame(string userToken)
        {
            return statusesFromUserId.GetOrAdd(
                       userToken,
                       new GameStatus(Guid.NewGuid().ToString(), true, GameStatus.Status.IN_PROGRESS, null)).GetId();
        }
    }
}
