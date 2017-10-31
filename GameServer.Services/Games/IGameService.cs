using SandTigerShark.GameServer.Services.Commands;
using SandTigerShark.GameServer.Services.Dtos;
using System;
using System.Threading.Tasks;

namespace SandTigerShark.GameServer.Services.Games
{
    public interface IGameService
    {
        Task<Guid> CreateGame(CreateGame command, Guid userToken);
        Task<Guid> GetAvailableGame(GameType gameType, Guid userToken);
        Task<GameStatus> GetGameStatus(Guid gameId, Guid userToken);
        Task Play(Guid gameId, Play command, Guid userToken);
    }
}