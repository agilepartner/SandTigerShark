using SandTigerShark.Services.Models;
using System;
using System.Threading.Tasks;

namespace GameServer.Services.Repositories
{
    public interface IGameRepository
    {
        // Find (first) available game.
        Guid GetAvailableGame();

        // Create a new game.
        void CreateGame();

        Task<GameStatus> GetGameStatus(string gameId);

    }
}