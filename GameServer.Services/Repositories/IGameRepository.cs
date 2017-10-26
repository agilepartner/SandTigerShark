using SandTigerShark.Services.Models;
using System.Threading.Tasks;

namespace GameServer.Services.Repositories
{
    public interface IGameRepository
    {
        Task<GameStatus> GetGameStatus(string gameId);
        string GetOrCreateNewGame(string userToken);
    }
}