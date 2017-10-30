using System;

namespace SandTigerShark.GameServer.Services.Games
{
    internal interface IGameFactory
    {
        Game Create(GameType type, Guid userToken);
    }
}