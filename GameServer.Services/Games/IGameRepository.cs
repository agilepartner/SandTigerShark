using System;
using System.Threading.Tasks;

namespace SandTigerShark.GameServer.Services.Games
{
    internal interface IGameRepository
    {
        Task<Guid> GetAvailableGame();
        Task Save(Game game);
        Task<Game> GetById(Guid gameId);
    }
}