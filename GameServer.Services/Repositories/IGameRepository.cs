using SandTigerShark.Services.Models;
using System.Threading.Tasks;

namespace GameServer.Services.Repositories
{
    public interface IGameRepository
    {
        // Find (first) available game.
        string GetAvailableGame();

        // Create a new game.
        void CreateGame();

        Task<GameStatus> GetGameStatus(string gameId);

    }
}