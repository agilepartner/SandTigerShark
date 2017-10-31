using System;
using System.Threading.Tasks;

namespace SandTigerShark.GameServer.Services.Games
{
    public interface IGameRepository
    {
        Task<Guid> GetAvailableGame(GameType gameType);
        Task Save(Game game);
        Task<Game> GetById(Guid gameId);
    }
}