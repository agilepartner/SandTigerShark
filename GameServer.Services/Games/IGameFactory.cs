using System;

namespace SandTigerShark.GameServer.Services.Games
{
    public interface IGameFactory
    {
        Game Create(GameType type, Guid userToken);
    }
}