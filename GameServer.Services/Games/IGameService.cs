using SandTigerShark.GameServer.Services.Commands;
using SandTigerShark.GameServer.Services.Dtos;
using System;
using System.Threading.Tasks;

namespace SandTigerShark.GameServer.Services.Games
{
    public interface IGameService
    {
        Task<Guid> GetAvailableGame(GameType gameType, Guid userToken);
        Task<Guid> CreateGame(CreateGame command, Guid userToken);
        Task<GameStatus> GetGameStatus(Guid gameId, Guid userToken);
        Task Play(Play command, Guid userToken);
    }
}