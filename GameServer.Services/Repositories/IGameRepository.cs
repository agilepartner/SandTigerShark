using SandTigerShark.Services.Models;
using System;
using System.Threading.Tasks;

namespace GameServer.Services.Repositories
{
    public interface IGameRepository
    {
        Task<Guid> GetAvailableGame();
        Task CreateGame();
        Task<GameStatus> GetGameStatus(Guid gameId, Guid userToken);
    }
}