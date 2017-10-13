using System.Threading.Tasks;
using SandTigerShark.Models;

namespace GameServer.Repositories
{
    public interface IGameRepository
    {
        Task<GameStatus> GetGameStatus(string gameId);
        string GetOrCreateNewGame(string userToken);
    }
}