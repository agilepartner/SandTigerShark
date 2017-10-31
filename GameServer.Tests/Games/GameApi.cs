using SandTigerShark.GameServer.Services.Games;
using System;

namespace SandTigerShark.GameServer.Tests.Games
{
    internal static class GameApi
    {
        public static Game WithPlayer2(this Game game, Guid? player2 = null)
        {
            game.AddPlayer(player2 ?? Guid.NewGuid());
            return game;
        }
    }
}