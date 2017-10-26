using SandTigerShark.Services.Models;
using System.Threading.Tasks;

namespace GameServer.Services.Repositories
{
    public interface IGameRepository
    {
        string GetAvailableGame();
        void CreateGame();
        Task<GameStatus> GetGameStatus(string gameId, string userToken);
    }
}