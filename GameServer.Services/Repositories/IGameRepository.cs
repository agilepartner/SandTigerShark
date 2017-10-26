using SandTigerShark.Services.Models;
using System.Threading.Tasks;

namespace GameServer.Services.Repositories
{
    public interface IGameRepository
    {
        // Find (first) available game.
        string availableGame();

        // Create a new game.
        void createGame();

        Task<GameStatus> GetGameStatus(string gameId);
        string GetOrCreateNewGame(string userToken);
    }
}